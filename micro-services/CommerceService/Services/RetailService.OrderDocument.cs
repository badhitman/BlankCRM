////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public partial class RetailService : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRetailDocumentAsync(CreateDocumentRetailRequestModel req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();

        if (req.WarehouseId <= 0)
        {
            res.AddError("Не указан склад");
            return res;
        }

        if (string.IsNullOrWhiteSpace(req.AuthorIdentityUserId))
        {
            res.AddError("Не указан автор/создатель документа");
            return res;
        }

        if (string.IsNullOrWhiteSpace(req.BuyerIdentityUserId))
        {
            res.AddError("Не указан покупатель");
            return res;
        }
        req.ExternalDocumentId = string.IsNullOrWhiteSpace(req.ExternalDocumentId)
            ? null
            : Regex.Replace(req.ExternalDocumentId, @"\s+", "");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (!string.IsNullOrWhiteSpace(req.ExternalDocumentId) && await context.OrdersRetail.AnyAsync(x => x.ExternalDocumentId == req.ExternalDocumentId, cancellationToken: token))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = $"Документ [ext #:{req.ExternalDocumentId}] уже существует" }] };

        TResponseModel<UserInfoModel[]> getUsers = await identityRepo.GetUsersOfIdentityAsync([req.AuthorIdentityUserId, req.BuyerIdentityUserId], token);
        if (!getUsers.Success())
        {
            res.AddRangeMessages(getUsers.Messages);
            return res;
        }
        List<RowOfRetailOrderDocumentModelDB> rowsDump = [];
        if (req.Rows is not null && req.Rows.Count != 0)
        {
            rowsDump = GlobalTools.CreateDeepCopy(req.Rows)!;
            req.Rows.ForEach(r =>
            {
                r.Order = req;
                r.Offer = null;
                r.Nomenclature = null;
            });
        }

        req.Version = Guid.NewGuid();
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;
        req.DateDocument = req.DateDocument.SetKindUtc();

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        DocumentRetailModelDB docDb = DocumentRetailModelDB.Build(req);

        await context.OrdersRetail.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.AddSuccess($"Заказ успешно создан #{docDb.Id}");
        res.Response = docDb.Id;

        if (req.InjectToPaymentId > 0)
        {
            await context.PaymentsOrdersLinks.AddAsync(new()
            {
                OrderDocumentId = req.Id,
                PaymentDocumentId = req.InjectToPaymentId,
                AmountPayment = req.Rows is null || req.Rows.Count == 0
                    ? 0
                    : req.Rows.Sum(x => x.Amount)
            }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo("Создана связь заказа с платежом");
        }

        if (req.InjectToConversionId > 0)
        {
            await context.ConversionsOrdersLinksRetail.AddAsync(new() { OrderDocumentId = req.Id, ConversionDocumentId = req.InjectToConversionId }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo("Создана связь заказа с переводом/конвертацией");
        }

        if (req.InjectToDeliveryId > 0)
        {
            await context.OrdersDeliveriesLinks.AddAsync(new()
            {
                OrderDocumentId = req.Id,
                DeliveryDocumentId = req.InjectToDeliveryId,
                WeightShipping = rowsDump.Count == 0 ? 0 : rowsDump.Sum(x => x.WeightOffer)
            }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo("Создана связь заказа с отгрузкой/доставкой");
        }

        await transaction.CommitAsync(token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRetailDocumentAsync(DocumentRetailModelDB req, CancellationToken token = default)
    {
        if (req.WarehouseId <= 0)
            return ResponseBaseModel.CreateError("Не указан склад");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        string msg;
        req.ExternalDocumentId = req.ExternalDocumentId?.Trim();
        if (!string.IsNullOrWhiteSpace(req.ExternalDocumentId) && await context.OrdersRetail.AnyAsync(x => x.Id != req.Id && x.ExternalDocumentId == req.ExternalDocumentId, cancellationToken: token))
            return ResponseBaseModel.CreateError($"Документ [ext #:{req.ExternalDocumentId}] уже существует");

        DocumentRetailModelDB documentDb = await context.OrdersRetail
            .Include(x => x.Rows)
            .FirstAsync(x => x.Id == req.Id, cancellationToken: token);

        if (documentDb.Version != req.Version)
            return ResponseBaseModel.CreateError($"Документ уже был кем-то изменён. Обновите документ и попробуйте снова его изменить");

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!offOrdersStatuses.Contains(documentDb.StatusDocument) && documentDb.WarehouseId != req.WarehouseId)
        {
            List<LockTransactionModelDB> offersLocked = [];
            foreach (RowOfRetailOrderDocumentModelDB rowDoc in documentDb.Rows!)
            {
                offersLocked.AddRange(new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = req.WarehouseId,
                    LockerId = rowDoc.OfferId,
                },
                new LockTransactionModelDB()
                {
                    LockerName = nameof(OfferAvailabilityModelDB),
                    LockerAreaId = documentDb.WarehouseId,
                    LockerId = rowDoc.OfferId,
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
                    loggerRepo.LogError(ex, $"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                    return ResponseBaseModel.CreateError($"{msg}{ex.Message}");
                }
            }

            ResponseBaseModel res = new();
            int[] _offersIds = [.. documentDb.Rows.Select(x => x.OfferId).Distinct()];
            List<OfferAvailabilityModelDB> registersOffersDb = await context.OffersAvailability
                .Where(x => _offersIds.Any(y => y == x.OfferId))
                .Include(x => x.Offer)
                .Include(x => x.Nomenclature)
                .ToListAsync(cancellationToken: token);

            TResponseModel<bool?> res_WarehouseReserveForRetailOrder = await StorageTransmissionRepo
                .ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.WarehouseReserveForRetailOrder, token);

            foreach (RowOfRetailOrderDocumentModelDB rowOfDocument in documentDb.Rows)
            {
                OfferAvailabilityModelDB?
                    registerOffer = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == req.WarehouseId),
                    registerOfferWriteOff = registersOffersDb.FirstOrDefault(x => x.OfferId == rowOfDocument.OfferId && x.WarehouseId == documentDb.WarehouseId);

                if (res_WarehouseReserveForRetailOrder.Response == true)
                {
                    if (registerOfferWriteOff is null)
                    {
                        registerOfferWriteOff = new()
                        {
                            WarehouseId = documentDb.WarehouseId,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                            Quantity = rowOfDocument.Quantity,
                        };
                        await context.OffersAvailability.AddAsync(registerOfferWriteOff, token);
                        await context.SaveChangesAsync(token);
                        registersOffersDb.Add(registerOfferWriteOff);
                    }
                    else
                        await context.OffersAvailability
                            .Where(x => x.Id == registerOfferWriteOff.Id)
                            .ExecuteUpdateAsync(x => x.SetProperty(p => p.Quantity, p => p.Quantity + rowOfDocument.Quantity), cancellationToken: token);
                }
                else
                {
                    if (registerOfferWriteOff is not null)
                    {
                        if (registerOfferWriteOff.Quantity < rowOfDocument.Quantity)
                        {
                            msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток {registerOfferWriteOff.Quantity})";
                            loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
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
                        msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        break;
                    }
                }

                if (res_WarehouseReserveForRetailOrder.Response == true)
                {
                    if (registerOffer is null)
                    {
                        msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток отсутствует)";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        return res;
                    }
                    else if (registerOffer.Quantity < rowOfDocument.Quantity)
                    {
                        msg = $"Количество [offer: #{rowOfDocument.OfferId} '{rowOfDocument.Offer?.GetName()}'] не может быть списано (остаток {registerOffer.Quantity})";
                        loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                        res.AddError($"{msg}. Баланс не может быть отрицательным");
                        return res;
                    }
                    else
                        await context.OffersAvailability
                            .Where(x => x.Id == registerOffer.Id)
                            .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Quantity, p => p.Quantity - rowOfDocument.Quantity), cancellationToken: token);
                }
                else
                {
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
                            WarehouseId = req.WarehouseId,
                            NomenclatureId = rowOfDocument.NomenclatureId,
                            OfferId = rowOfDocument.OfferId,
                            Quantity = rowOfDocument.Quantity,
                        };
                        await context.OffersAvailability.AddAsync(registerOffer, cancellationToken: token);
                    }
                }
            }

            if (!res.Success())
            {
                await transaction.RollbackAsync(token);
                msg = $"Не удалось обновить складской документ: ";
                loggerRepo.LogWarning($"{msg}{JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
                res.AddError(msg);
                return res;
            }

            if (offersLocked.Count != 0)
            {
                context.RemoveRange(offersLocked);
                await context.SaveChangesAsync(token);
            }
        }

        await context.OrdersRetail
              .Where(x => x.Id == req.Id)
              .ExecuteUpdateAsync(set => set
                  .SetProperty(p => p.Name, req.Name.Trim())
                  .SetProperty(p => p.NumWeekOfYear, req.NumWeekOfYear)
                  .SetProperty(p => p.DateDocument, req.DateDocument.SetKindUtc())
                  .SetProperty(p => p.BuyerIdentityUserId, req.BuyerIdentityUserId)
                  .SetProperty(p => p.Version, Guid.NewGuid())
                  .SetProperty(p => p.Description, req.Description)
                  .SetProperty(p => p.WarehouseId, req.WarehouseId)
                  .SetProperty(p => p.ExternalDocumentId, req.ExternalDocumentId)
                  .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);


        await transaction.CommitAsync(token);

        return ResponseBaseModel.CreateSuccess("Документ/заказ обновлён");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DocumentRetailModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DocumentRetailModelDB> q = context.OrdersRetail.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x =>
                x.Name.Contains(req.FindQuery) ||
                (x.Description != null && x.Description.Contains(req.FindQuery)) ||
                (x.ExternalDocumentId != null && x.ExternalDocumentId.Contains(req.FindQuery)));

        if (req.Payload?.BuyersFilterIdentityId is not null && req.Payload.BuyersFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.BuyersFilterIdentityId.Contains(x.BuyerIdentityUserId));

        if (req.Payload?.CreatorsFilterIdentityId is not null && req.Payload.CreatorsFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.CreatorsFilterIdentityId.Contains(x.AuthorIdentityUserId));

        if (req.Payload?.ExcludeDeliveryId.HasValue == true && req.Payload.ExcludeDeliveryId > 0)
            q = q.Where(x => !context.OrdersDeliveriesLinks.Any(y => y.OrderDocumentId == x.Id && y.DeliveryDocumentId == req.Payload.ExcludeDeliveryId));

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Count != 0)
        {
            bool _unsetChecked = req.Payload.StatusesFilter.Contains(null);
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.StatusDocument) || (_unsetChecked && x.StatusDocument == 0));
        }

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DateDocument >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.Payload.End);
        }

        if (req.Payload is not null && req.Payload.EqualsSumFilter == true)
            q = q.Where(x => context.RowsOrdersRetails.Where(y => y.OrderId == x.Id).Sum(y => y.Amount) != (context.PaymentsOrdersLinks.Where(y => y.OrderDocumentId == x.Id && context.PaymentsRetailDocuments.Any(z => z.StatusPayment == PaymentsRetailStatusesEnum.Paid && z.Id == y.PaymentDocumentId)).Sum(y => y.AmountPayment) + context.ConversionsOrdersLinksRetail.Where(y => y.OrderDocumentId == x.Id && context.ConversionsDocumentsWalletsRetail.Any(z => z.Id == y.ConversionDocumentId && !z.IsDisabled)).Sum(y => y.AmountPayment)));

        IOrderedQueryable<DocumentRetailModelDB> oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => q.OrderBy(x => x.DateDocument),
            DirectionsEnum.Down => q.OrderByDescending(x => x.DateDocument),
            _ => q.OrderBy(x => x.Name)
        };

        IQueryable<DocumentRetailModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<DocumentRetailModelDB> res = await pq
                .Include(x => x.Rows!)
                .ThenInclude(x => x.Offer)

                .Include(x => x.Deliveries!)
                .ThenInclude(x => x.DeliveryDocument)

                .Include(x => x.Conversions!)
                .ThenInclude(x => x.ConversionDocument)

                .Include(x => x.Payments!)
                .ThenInclude(x => x.PaymentDocument)

                .ToListAsync(cancellationToken: token);

        res.ForEach(x => { if (x.StatusDocument == 0 || x.StatusDocument == default) x.StatusDocument = null; });

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
    public async Task<TResponseModel<DocumentRetailModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DocumentRetailModelDB> q = context.OrdersRetail
            .Where(x => req.Ids.Contains(x.Id));

        TResponseModel<DocumentRetailModelDB[]> res = new()
        {
            Response = !req.IncludeDataExternal
                ? await q.ToArrayAsync(cancellationToken: token)
                : await q.Include(x => x.Rows!)
                         .ThenInclude(x => x.Offer)

                         .Include(x => x.Deliveries!)
                         .ThenInclude(x => x.DeliveryDocument)

                         .Include(x => x.Conversions!)
                         .ThenInclude(x => x.ConversionDocument)

                         .Include(x => x.Payments!)
                         .ThenInclude(x => x.PaymentDocument)

                         .ToArrayAsync(cancellationToken: token)
        };

        if (res.Response.Length != req.Ids.Length)
            res.AddError("Некоторые документы не найдены");

        return res;
    }
}