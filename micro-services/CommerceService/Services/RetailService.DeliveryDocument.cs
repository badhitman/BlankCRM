////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public partial class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryDocumentAsync(TAuthRequestStandardModel<CreateDeliveryDocumentRetailRequestModel> req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (req.Payload is null)
        {
            res.AddError("req.Payload is null");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        TResponseModel<KladrResponseModel> kladrObj = string.IsNullOrWhiteSpace(req.Payload.KladrCode)
            ? new()
            : await kladrRepo.ObjectGetAsync(new() { Code = req.Payload.KladrCode }, token);

        if (!kladrObj.Success())
        {
            res.AddRangeMessages(kladrObj.Messages);
            return res;
        }

        TResponseModel<UserInfoModel[]> user = await identityRepo.GetUsersOfIdentityAsync([req.Payload.RecipientIdentityUserId], token);
        if (!user.Success())
        {
            res.AddRangeMessages(user.Messages);
            return res;
        }

        req.Payload.CreatedAtUTC = DateTime.UtcNow;
        req.Payload.OrdersLinks = null;
        req.Payload.Name = req.Payload.Name.Trim();
        req.Payload.Description = req.Payload.Description?.Trim();
        req.Payload.DeliveryCode = req.Payload.DeliveryCode?.Trim();
        req.Payload.Version = Guid.NewGuid();

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        DeliveryDocumentRetailModelDB docDb = DeliveryDocumentRetailModelDB.Build(req.Payload);

        await context.DeliveryDocumentsRetail.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.Response = docDb.Id;
        res.AddSuccess($"Документ отгрузки/доставки создан #{docDb.Id}");

        if (req.Payload.InjectToOrderId > 0)
        {
            TraceReceiverRecord trace = TraceReceiverRecord.Build(GlobalStaticConstantsTransmission.TransmissionQueues.CreateConversionOrderLinkDocumentRetailReceive, req.SenderActionUserId, new RetailOrderDeliveryLinkModelDB()
            {
                DeliveryDocumentId = docDb.Id,
                WeightShipping = req.Payload.WeightShipping,
                OrderDocumentId = req.Payload.InjectToOrderId,
            });

            RetailOrderDeliveryLinkModelDB _retailOrderDeliveryLink = new()
            {
                DeliveryDocumentId = docDb.Id,
                OrderDocumentId = req.Payload.InjectToOrderId,
                WeightShipping = req.Payload.WeightShipping,
            };
            await context.OrdersDeliveriesLinks.AddAsync(_retailOrderDeliveryLink, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь документа отгрузки/доставки #{docDb.Id} с заказом #{req.Payload.InjectToOrderId}");
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(new TResponseModel<int>()
            {
                Response = _retailOrderDeliveryLink.Id,
            }), token);
        }

        await transaction.CommitAsync(token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>> UpdateDeliveryDocumentAsync(TAuthRequestStandardModel<DeliveryDocumentRetailModelDB> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "req.Payload is null" }] };

        req.Payload.CreatedAtUTC = DateTime.UtcNow;
        req.Payload.OrdersLinks = null;
        req.Payload.Name = req.Payload.Name.Trim();
        req.Payload.Description = req.Payload.Description?.Trim();
        req.Payload.DeliveryCode = req.Payload.DeliveryCode?.Trim();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        DeliveryDocumentRetailModelDB documentDb = await context.DeliveryDocumentsRetail
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.Payload.Id, cancellationToken: token);

        if (documentDb.Version != req.Payload.Version)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Документ уже был кем-то изменён. Обновите документ и попробуйте снова его изменить" }] };

        loggerRepo.LogInformation($"{nameof(documentDb)}: {JsonConvert.SerializeObject(documentDb, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

        TResponseModel<Guid?> res = new();
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        string msg;
        if (!offDeliveriesStatuses.Contains(documentDb.DeliveryStatus) && documentDb.WarehouseId != req.Payload.WarehouseId)
        {
            List<LockTransactionModelDB> offersLocked = [];
            foreach (RowOfDeliveryRetailDocumentModelDB rowDoc in documentDb.Rows!)
            {
                offersLocked.AddRange(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = req.Payload.WarehouseId,
                    LockerId = rowDoc.OfferId,
                    Marker = nameof(UpdateDeliveryDocumentAsync),
                },
                new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = documentDb.WarehouseId,
                    LockerId = rowDoc.OfferId,
                    Marker = nameof(UpdateDeliveryDocumentAsync),
                });
            }
            if (offersLocked.Count != 0)
            {
                try
                {
                    await context.AddRangeAsync(offersLocked, token);
                    await context.SaveChangesAsync(token);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Не удалось выполнить команду блокировки регистров остатков {nameof(UpdateRetailDocumentAsync)}: ";
                    loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"{msg}{ex.Message}" }] };
                }
            }

            int[] _offersIds = [.. documentDb.Rows.Select(x => x.OfferId).Distinct()];
            List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
                .Where(x => _offersIds.Any(y => y == x.OfferId))
                .Include(x => x.Offer)
                .Include(x => x.Nomenclature)
                .ToListAsync(cancellationToken: token);

            foreach (RowOfDeliveryRetailDocumentModelDB rowOfDocument in documentDb.Rows)
            {
                OfferAvailabilityModelDB?
                    registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.Payload.WarehouseId),
                    registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == documentDb.WarehouseId);

                if (registerOfferWriteOff is not null)
                {
                    if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity)
                    {
                        await transaction.RollbackAsync(token);
                        msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток {registerOfferWriteOff.Quantity})";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        break;
                    }

                    await context.OffersAvailability
                        .Where(y => y.Id == registerOfferWriteOff.Id)
                        .ExecuteUpdateAsync(set =>
                            set.SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocument.Quantity), cancellationToken: token);

                    registerOfferWriteOff.Quantity -= rowOfDocument.Quantity;
                }
                else if (documentDb.WarehouseId > 0)
                {
                    await transaction.RollbackAsync(token);
                    msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                    loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    res.AddError($"{msg}. Баланс не может быть отрицательным");
                    break;
                }

                if (registerOffer is not null)
                {
                    await context.OffersAvailability
                         .Where(y => y.Id == registerOffer.Id)
                         .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity + rowOfDocument.Quantity), cancellationToken: token);

                    registerOffer.Quantity += rowOfDocument.Quantity;
                }
                else
                {
                    registerOffer = new OfferAvailabilityModelDB()
                    {
                        WarehouseId = req.Payload.WarehouseId,
                        NomenclatureId = rowOfDocument.NomenclatureId,
                        OfferId = rowOfDocument.OfferId,
                        Quantity = rowOfDocument.Quantity,
                    };
                    await context.OffersAvailability.AddAsync(registerOffer, cancellationToken: token);
                    await context.SaveChangesAsync(token);
                    registersOffersDb.Add(registerOffer);
                }
            }

            if (!res.Success())
            {
                await transaction.RollbackAsync(token);
                msg = $"Не удалось обновить складской документ: ";
                loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req.Payload, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError(msg);
                return res;
            }

            if (offersLocked.Count != 0)
            {
                context.LockTransactions.RemoveRange(offersLocked);
                await context.SaveChangesAsync(token);
            }
        }
        res.Response = Guid.NewGuid();
        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.Payload.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Payload.Name)
                .SetProperty(p => p.Description, req.Payload.Description)
                .SetProperty(p => p.WeightShipping, req.Payload.WeightShipping)
                .SetProperty(p => p.ShippingCost, req.Payload.ShippingCost)
                .SetProperty(p => p.PackageSize, req.Payload.PackageSize)
                .SetProperty(p => p.Notes, req.Payload.Notes)
                .SetProperty(p => p.RecipientIdentityUserId, req.Payload.RecipientIdentityUserId)
                .SetProperty(p => p.KladrTitle, req.Payload.KladrTitle)
                .SetProperty(p => p.KladrCode, req.Payload.KladrCode)
                .SetProperty(p => p.DeliveryType, req.Payload.DeliveryType)
                .SetProperty(p => p.DeliveryPaymentUponReceipt, req.Payload.DeliveryPaymentUponReceipt)
                .SetProperty(p => p.DeliveryCode, req.Payload.DeliveryCode)
                .SetProperty(p => p.Version, res.Response)
                .SetProperty(p => p.AddressUserComment, req.Payload.AddressUserComment)
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.Payload.Id).OrderByDescending(z => z.DateOperation).ThenByDescending(os => os.Id).Select(s => s.DeliveryStatus).FirstOrDefault())
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);
        res.AddSuccess("Ok");
        return res;
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryDocumentRetailModelDB> q = context.DeliveryDocumentsRetail.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x =>
                x.Name.Contains(req.FindQuery) ||
                (x.Description != null && x.Description.Contains(req.FindQuery)) ||
                (x.DeliveryCode != null && x.DeliveryCode.Contains(req.FindQuery)));

        if (req.Payload?.RecipientsFilterIdentityId is not null && req.Payload.RecipientsFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.RecipientsFilterIdentityId.Contains(x.RecipientIdentityUserId));

        if (req.Payload?.TypesFilter is not null && req.Payload.TypesFilter.Length != 0)
            q = q.Where(x => req.Payload.TypesFilter.Contains(x.DeliveryType));

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Count != 0)
        {
            bool selectedUnset = req.Payload.StatusesFilter.Contains(null);
            req.Payload.StatusesFilter = [.. req.Payload.StatusesFilter.Where(x => x is not null).Distinct()];
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.DeliveryStatus) || (selectedUnset && (x.DeliveryStatus == null || x.DeliveryStatus == 0)));
        }

        if (req.Payload?.ExcludeOrderId.HasValue == true && req.Payload.ExcludeOrderId > 0)
            q = q.Where(x => !context.OrdersDeliveriesLinks.Any(y => y.DeliveryDocumentId == x.Id && y.OrderDocumentId == req.Payload.ExcludeOrderId));

        if (req.Payload?.FilterOrderId is not null && req.Payload.FilterOrderId > 0)
            q = from deliveryDoc in q
                join linkItem in context.OrdersDeliveriesLinks.Where(x => x.OrderDocumentId == req.Payload.FilterOrderId) on deliveryDoc.Id equals linkItem.DeliveryDocumentId
                select deliveryDoc;

        if (req.Payload?.Start is not null && req.Payload.Start != default)
        {
            req.Payload.Start = req.Payload.Start.SetKindUtc();
            q = q.Where(x => x.CreatedAtUTC >= req.Payload.Start);
        }

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.CreatedAtUTC <= req.Payload.End);
        }

        if (req.Payload?.EqualSumFilter == true)
            q = q.Where(x => context.RowsDeliveryDocumentsRetail.Where(y => x.Id == y.DocumentId).Sum(y => y.WeightOffer) != context.OrdersDeliveriesLinks.Where(y => x.Id == y.OrderDocumentId).Sum(y => y.WeightShipping));

        IOrderedQueryable<DeliveryDocumentRetailModelDB> oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => q.OrderBy(x => x.CreatedAtUTC),
            DirectionsEnum.Down => q.OrderByDescending(x => x.CreatedAtUTC),
            _ => q.OrderBy(x => x.Name)
        };

        IQueryable<DeliveryDocumentRetailModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<DeliveryDocumentRetailModelDB> res = await pq
                .Include(x => x.Rows!)
                .ThenInclude(x => x.Offer)
                .Include(x => x.DeliveryStatusesLog)
                .Include(x => x.OrdersLinks)
                .ToListAsync(cancellationToken: token);

        if (res.Count != 0)
            res.ForEach(x => { if (x.DeliveryStatus == 0) { x.DeliveryStatus = null; } });

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = res
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB[]>> GetDeliveryDocumentsAsync(GetDeliveryDocumentsRetailRequestModel req, CancellationToken token = default)
    {
        if (req.Ids is null || req.Ids.Length == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Ids is null || Ids.Length == 0" }] };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context.DeliveryDocumentsRetail
                .Where(x => req.Ids.Contains(x.Id))
                .Include(x => x.Rows!)
                .ThenInclude(x => x.Offer!)
                .ThenInclude(x => x.Nomenclature)
                .ToArrayAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<FileAttachModel> GetDeliveriesJournalFileAsync(SelectDeliveryDocumentsRetailRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryDocumentRetailModelDB> q = context.DeliveryDocumentsRetail.AsQueryable();

        if (req.RecipientsFilterIdentityId is not null && req.RecipientsFilterIdentityId.Length != 0)
            q = q.Where(x => req.RecipientsFilterIdentityId.Contains(x.RecipientIdentityUserId));

        if (req.TypesFilter is not null && req.TypesFilter.Length != 0)
            q = q.Where(x => req.TypesFilter.Contains(x.DeliveryType));

        if (req.StatusesFilter is not null && req.StatusesFilter.Count != 0)
        {
            bool selectedUnset = req.StatusesFilter.Contains(null);
            req.StatusesFilter = [.. req.StatusesFilter.Where(x => x is not null).Distinct()];
            q = q.Where(x => req.StatusesFilter.Contains(x.DeliveryStatus) || (selectedUnset && (x.DeliveryStatus == null || x.DeliveryStatus == 0)));
        }

        if (req.ExcludeOrderId.HasValue == true && req.ExcludeOrderId > 0)
            q = q.Where(x => !context.OrdersDeliveriesLinks.Any(y => y.DeliveryDocumentId == x.Id && y.OrderDocumentId == req.ExcludeOrderId));

        if (req.FilterOrderId is not null && req.FilterOrderId > 0)
            q = from deliveryDoc in q
                join linkItem in context.OrdersDeliveriesLinks.Where(x => x.OrderDocumentId == req.FilterOrderId) on deliveryDoc.Id equals linkItem.DeliveryDocumentId
                select deliveryDoc;

        if (req.Start is not null && req.Start != default)
        {
            req.Start = req.Start.SetKindUtc();
            q = q.Where(x => x.CreatedAtUTC >= req.Start);
        }

        if (req.End is not null && req.End != default)
        {
            req.End = req.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.CreatedAtUTC <= req.End);
        }

        if (req.EqualSumFilter == true)
            q = q.Where(x => context.RowsDeliveryDocumentsRetail.Where(y => x.Id == y.DocumentId).Sum(y => y.WeightOffer) != context.OrdersDeliveriesLinks.Where(y => x.Id == y.OrderDocumentId).Sum(y => y.WeightShipping));

        q = q.OrderBy(x => x.CreatedAtUTC);

        List<DeliveryDocumentRetailModelDB> resDb = await q
                .Include(x => x.Rows!)
                .ThenInclude(x => x.Offer)
                .Include(x => x.DeliveryStatusesLog)
                .Include(x => x.OrdersLinks)
                .ToListAsync(cancellationToken: token);

        string[] usersList = [.. resDb.Select(x => x.RecipientIdentityUserId).Distinct()];
        TResponseModel<UserInfoModel[]> users = await identityRepo.GetUsersOfIdentityAsync(usersList, token);
        UserInfoModel[] usersDb = users.Response ?? throw new("users for deliveries journal - IS NULL");

        string docName = $"Журнал отгрузки {DateTime.Now.GetHumanDateTime()}";
        FileAttachModel res;

        HtmlGenerator.html5.areas.div wrapDiv = new();
        // wrapDiv.AddDomNode(new HtmlGenerator.html5.textual.p(docName));

        resDb.ForEach(aNode =>
        {
            HtmlGenerator.html5.forms.fieldset addressDiv = new($"#{aNode.Id} {aNode.Name} {aNode.DeliveryStatus} (заявлено {aNode.WeightShipping} kg)".Trim().Replace("  ", " "), $"doc_{aNode.Id}");

            addressDiv.AddDomNode(new HtmlGenerator.html5.textual.strong("Адрес:"));
            addressDiv.AddDomNode(new HtmlGenerator.html5.textual.span($"`{aNode.KladrTitle}` {aNode.AddressUserComment}".Trim()));
            addressDiv.AddDomNode(new HtmlGenerator.html5.textual.strong("Получатель"));
            addressDiv.AddDomNode(new HtmlGenerator.html5.textual.span($"{usersDb.First(x => x.UserId == aNode.RecipientIdentityUserId)}".Trim()));

            HtmlGenerator.html5.tables.table my_table = new() { css_style = "border: 1px solid black; width: 100%; border-collapse: collapse;" };

            my_table
                .THead.AddColumn("Наименование")
                .AddColumn("Цена")
                .AddColumn("Кол-во")
                .AddColumn("Вес")
                .AddColumn("Сумма");

            aNode.Rows?.ForEach(dr =>
            {
                my_table.TBody.AddRow(
                    [dr.Offer!.GetName(),
                            dr.Offer.Price.ToString(),
                            dr.Quantity.ToString(),
                            dr.WeightOffer.ToString(),
                            dr.Amount.ToString()]);
            });

            my_table.TBody.AddRow(["", "", "", $"Итого вес:{aNode.Rows!.Sum(x => x.WeightOffer)}", $"Итого сумма: {aNode.Rows!.Sum(x => x.Amount)}"]);

            addressDiv.AddDomNode(my_table);

            wrapDiv.AddDomNode(addressDiv);
        });

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string test_s = $"<style>table, th, td {{border: 1px solid black;border-collapse: collapse;}}</style>{wrapDiv.GetHTML()}";

        using MemoryStream ms = new();
        StreamWriter writer = new(ms);
        writer.Write(test_s);
        writer.Flush();
        ms.Position = 0;

        res = new()
        {
            Data = ms.ToArray(),
            ContentType = GlobalTools.ContentTypes.First(x => x.Value.Contains("html")).Key,
            Name = $"{docName.Replace(":", "-").Replace(" ", "_")}.html",
        };

        return res;
    }
}