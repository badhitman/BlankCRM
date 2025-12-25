////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public class RetailService(IIdentityTransmission identityRepo,
    ILogger<RetailService> loggerRepo,
    IKladrNavigationService kladrRepo,
    IDbContextFactory<CommerceContext> commerceDbFactory) : IRetailService
{
    #region Delivery document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryDocumentAsync(CreateDeliveryDocumentRetailRequestModel req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        TResponseModel<KladrResponseModel> kladrObj = await kladrRepo.ObjectGetAsync(new() { Code = req.KladrCode }, token);
        if (!kladrObj.Success())
        {
            res.AddRangeMessages(kladrObj.Messages);
            return res;
        }

        TResponseModel<UserInfoModel[]> user = await identityRepo.GetUsersOfIdentityAsync([req.RecipientIdentityUserId], token);
        if (!user.Success())
        {
            res.AddRangeMessages(user.Messages);
            return res;
        }

        req.CreatedAtUTC = DateTime.UtcNow;
        req.OrdersLinks = null;
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.DeliveryCode = req.DeliveryCode?.Trim();

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        DeliveryDocumentRetailModelDB docDb = DeliveryDocumentRetailModelDB.Build(req);

        await context.DeliveryDocumentsRetail.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.Response = docDb.Id;
        res.AddSuccess($"Документ отгрузки/доставки создан #{docDb.Id}");

        if (req.InjectToOrderId > 0)
        {
            await context.OrdersDeliveriesLinks.AddAsync(new()
            {
                DeliveryDocumentId = docDb.Id,
                OrderDocumentId = req.InjectToOrderId,
                WeightShipping = req.WeightShipping,
            }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь документа отгрузки/доставки #{docDb.Id} с заказом #{req.InjectToOrderId}");
        }

        await transaction.CommitAsync(token);

        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
    {
        req.CreatedAtUTC = DateTime.UtcNow;
        req.OrdersLinks = null;
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.DeliveryCode = req.DeliveryCode?.Trim();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.WeightShipping, req.WeightShipping)
                .SetProperty(p => p.ShippingCost, req.ShippingCost)
                .SetProperty(p => p.RecipientIdentityUserId, req.RecipientIdentityUserId)
                .SetProperty(p => p.KladrTitle, req.KladrTitle)
                .SetProperty(p => p.KladrCode, req.KladrCode)
                .SetProperty(p => p.DeliveryType, req.DeliveryType)
                .SetProperty(p => p.DeliveryPaymentUponReceipt, req.DeliveryPaymentUponReceipt)
                .SetProperty(p => p.DeliveryCode, req.DeliveryCode)
                .SetProperty(p => p.AddressUserComment, req.AddressUserComment)
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.Id).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault())
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
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
            addressDiv.AddDomNode(new HtmlGenerator.html5.textual.span($"{usersDb.First(x => x.UserId == aNode.RecipientIdentityUserId).ToString()}".Trim()));

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
    #endregion

    #region Row`s Of Delivery Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.Offer = null;
        req.Nomenclature = null;
        req.Offer = null;
        req.Document = null;
        req.Comment = req.Comment?.Trim();
        req.Version = Guid.NewGuid();

        await context.RowsDeliveryDocumentsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        req.Offer = null;
        req.Nomenclature = null;
        req.Offer = null;
        req.Document = null;
        req.Comment = req.Comment?.Trim();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.RowsDeliveryDocumentsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.OfferId, req.OfferId)
                .SetProperty(p => p.Quantity, req.Quantity)
                .SetProperty(p => p.Amount, req.Amount)
                .SetProperty(p => p.Comment, req.Comment)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.NomenclatureId, req.NomenclatureId), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowsOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Status = new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }] } };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfDeliveryRetailDocumentModelDB> q = context.RowsDeliveryDocumentsRetail.Where(x => x.DocumentId == req.Payload.DeliveryDocumentId).AsQueryable();

        IQueryable<RowOfDeliveryRetailDocumentModelDB> pq = q
            .OrderBy(x => x.Id)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<RowOfDeliveryRetailDocumentModelDB> res = await pq.Include(x => x.Offer).ToListAsync(cancellationToken: token);
        foreach (RowOfDeliveryRetailDocumentModelDB row in res.Where(x => x.Amount <= 0 || x.WeightOffer <= 0))
        {
            if (row.Amount <= 0)
                row.Amount = row.Quantity * row.Offer!.Price;

            if (row.WeightOffer <= 0)
                row.WeightOffer = row.Quantity * row.Offer!.Weight;

            context.Update(row);
            await context.SaveChangesAsync(token);
        }

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
    public async Task<ResponseBaseModel> DeleteRowOfDeliveryDocumentAsync(int rowId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.RowsDeliveryDocumentsRetail.Where(x => x.Id == rowId).ExecuteDeleteAsync(cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Элемент удалён");
    }
    #endregion

    #region Statuses (for delivery document)
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.DateOperation = req.DateOperation.SetKindUtc();
        req.DeliveryDocument = null;
        req.Name = req.Name.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        await context.DeliveriesStatusesDocumentsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveriesStatusesDocumentsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.DeliveryStatus, req.DeliveryStatus)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == req.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == req.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Status = new()
                {
                    Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }]
                }
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB> q = context.DeliveriesStatusesDocumentsRetail
            .Where(x => x.DeliveryDocumentId == req.Payload.DeliveryDocumentId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

        IQueryable<DeliveryStatusRetailDocumentModelDB>? pq = q
            .OrderBy(x => x.DateOperation)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq.ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryStatusDocumentAsync(int statusId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB> q = context.DeliveriesStatusesDocumentsRetail.Where(x => x.Id == statusId);
        int deliveryDocumentId = await q.Select(x => x.DeliveryDocumentId).FirstAsync(cancellationToken: token);

        await q.ExecuteDeleteAsync(cancellationToken: token);

        await context.DeliveryDocumentsRetail
            .Where(x => x.Id == deliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveriesStatusesDocumentsRetail.Where(y => y.DeliveryDocumentId == deliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Строка-статус успешно удалена");
    }
    #endregion

    #region Wallet Type
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (string.IsNullOrWhiteSpace(req.Name))
        {
            res.AddError("Укажите имя");
            return res;
        }

        req.Name = req.Name.Trim();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (await context.WalletsRetailTypes.AnyAsync(x => x.Name == req.Name, cancellationToken: token))
        {
            res.AddError("Тип кошелька с таким именем уже существует");
            return res;
        }

        req.Description = req.Description?.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        await context.WalletsRetailTypes.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        if (await context.WalletsRetailTypes.AnyAsync(x => x.Id != req.Id, cancellationToken: token))
        {
            req.SortIndex = await context.WalletsRetailTypes.MaxAsync(x => x.SortIndex, cancellationToken: token);
            req.SortIndex++;
            context.WalletsRetailTypes.Update(req);
            await context.SaveChangesAsync(token);
        }
        else
            req.SortIndex = 1;

        return new() { Response = req.Id };
    }

    /// <inheritdoc/> 
    public async Task<ResponseBaseModel> UpdateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
    {
        req.Description = req.Description?.Trim();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.WalletsRetailTypes
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name.Trim())
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.IsDisabled, req.IsDisabled)
                .SetProperty(p => p.IsSystem, req.IsSystem)
                .SetProperty(p => p.IgnoreBalanceChanges, req.IgnoreBalanceChanges)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletRetailTypeModelDB> q = context.WalletsRetailTypes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

        IQueryable<WalletRetailTypeModelDB>? pq = q
            .OrderBy(x => x.SortIndex)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<WalletRetailTypeModelDB> res = await pq
            .OrderBy(x => x.SortIndex)
            .Include(x => x.DisabledPaymentsTypes)
            .ToListAsync(cancellationToken: token);

        var sumRes = await pq
            .Select(x => new { x.Id, sum = context.WalletsRetail.Where(y => y.WalletTypeId == x.Id).Sum(z => z.Balance) })
            .ToArrayAsync(cancellationToken: token);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = [..res.Select(x => new WalletRetailTypeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsSystem = x.IsSystem,
                SortIndex = x.SortIndex,
                IsDisabled = x.IsDisabled,
                Description = x.Description,
                CreatedAtUTC = x.CreatedAtUTC,
                LastUpdatedAtUTC = x.LastUpdatedAtUTC,
                IgnoreBalanceChanges = x.IgnoreBalanceChanges,
                DisabledPaymentsTypes = x.DisabledPaymentsTypes,
                SumBalances = sumRes.Where(y => y.Id == x.Id).FirstOrDefault()?.sum ?? 0
            })],
        };
    }

    /// <inheritdoc/> 
    public async Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletRetailTypeModelDB> q = context.WalletsRetailTypes
            .Where(x => reqIds.Contains(x.Id));

        WalletRetailTypeModelDB[] res = await q
            .Include(x => x.DisabledPaymentsTypes)
            .ToArrayAsync(cancellationToken: token);

        var sumRes = await q
            .Select(x => new { x.Id, sum = context.WalletsRetail.Where(y => y.WalletTypeId == x.Id).Sum(z => z.Balance) })
            .ToArrayAsync(cancellationToken: token);

        return new()
        {
            Response = [..res.Select(x => new WalletRetailTypeViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsSystem = x.IsSystem,
                SortIndex = x.SortIndex,
                IsDisabled = x.IsDisabled,
                Description = x.Description,
                CreatedAtUTC = x.CreatedAtUTC,
                LastUpdatedAtUTC = x.LastUpdatedAtUTC,
                IgnoreBalanceChanges = x.IgnoreBalanceChanges,
                DisabledPaymentsTypes = x.DisabledPaymentsTypes,
                SumBalances = sumRes.Where(y => y.Id == x.Id).FirstOrDefault()?.sum ?? 0
            })]
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ToggleWalletTypeDisabledForPaymentTypeAsync(ToggleWalletTypeDisabledForPaymentTypeRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<DisabledPaymentTypeForWalletRetailTypeModelDB> q = context
            .DisabledPaymentsTypesForWallets
            .Where(x => x.WalletTypeId == req.WalletTypeId && x.PaymentType == req.PaymentType);

        if (await q.ExecuteDeleteAsync(cancellationToken: token) == 0)
        {
            await context
            .DisabledPaymentsTypesForWallets.AddAsync(new()
            {
                PaymentType = req.PaymentType,
                WalletTypeId = req.WalletTypeId,
            }, token);
            await context.SaveChangesAsync(token);
        }

        return ResponseBaseModel.CreateSuccess("Ok");
    }
    #endregion

    #region Wallet
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        TResponseModel<UserInfoModel[]> user = await identityRepo.GetUsersOfIdentityAsync([req.UserIdentityId], token);
        if (!user.Success())
        {
            res.AddRangeMessages(user.Messages);
            return res;
        }

        req.Version = Guid.NewGuid();
        req.Description = req.Description?.Trim();
        req.Name = req.Name.Trim();
        req.WalletType = null;

        await context.WalletsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        res.Response = req.Id;
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.WalletsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.WalletTypeId, req.WalletTypeId)
                .SetProperty(p => p.UserIdentityId, req.UserIdentityId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new()
            {
                Status = new()
                {
                    Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }]
                }
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletRetailModelDB> q = context.WalletsRetail.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

        if (req.Payload.UsersFilterIdentityId is not null && req.Payload.UsersFilterIdentityId.Length != 0)
        {
            TResponseModel<UserInfoModel[]> user = await identityRepo.GetUsersOfIdentityAsync(req.Payload.UsersFilterIdentityId, token);
            if (!user.Success())
            {
                return new()
                {
                    Status = new()
                    {
                        Messages = user.Messages
                    }
                };
            }

            q = q.Where(x => req.Payload.UsersFilterIdentityId.Any(y => y == x.UserIdentityId));
        }

        IQueryable<WalletRetailModelDB>? pq = q
            .OrderBy(x => x.Id)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<WalletRetailModelDB> walletsRes = await pq
            .Include(x => x.WalletType!)
            .ThenInclude(x => x.DisabledPaymentsTypes)
            .ToListAsync(cancellationToken: token);

        if (req.Payload.AutoGenerationWallets == true && req.Payload.UsersFilterIdentityId is not null && req.Payload.UsersFilterIdentityId.Length != 0)
        {
            TResponseModel<UserInfoModel[]> getUsers = await identityRepo.GetUsersOfIdentityAsync(req.Payload.UsersFilterIdentityId, token);
            if (!getUsers.Success() || getUsers.Response is null)
                return new() { Status = new() { Messages = getUsers.Messages } };

            List<WalletRetailTypeModelDB> walletsTypesDb = await context.WalletsRetailTypes
                .Where(x => !x.IsDisabled)
                .ToListAsync(cancellationToken: token);

            List<WalletRetailModelDB> createWallets = [];
            foreach (string userId in req.Payload.UsersFilterIdentityId)
            {
                walletsTypesDb.ForEach(walletType =>
                {
                    if (!walletsRes.Any(x => x.WalletTypeId == walletType.Id && x.UserIdentityId == userId) && (!walletType.IsSystem || getUsers.Response.First(u => u.UserId == userId).IsAdmin))
                    {
                        createWallets.Add(new()
                        {
                            UserIdentityId = userId,
                            CreatedAtUTC = DateTime.UtcNow,
                            Name = "~",
                            Version = Guid.NewGuid(),
                            WalletTypeId = walletType.Id,
                        });
                    }
                });
            }

            if (createWallets.Count != 0)
            {
                await context.WalletsRetail.AddRangeAsync(createWallets, token);
                await context.SaveChangesAsync(cancellationToken: token);
                walletsRes = await pq.Include(x => x.WalletType).ToListAsync(cancellationToken: token);
            }
        }

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = walletsRes,
        };
    }
    #endregion

    #region Payment document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(CreatePaymentRetailDocumentRequestModel req, CancellationToken token = default)
    {
        if (req.Amount <= 0)
            return new()
            {
                Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Сумма платежа должна быть больше нуля" }]
            };

        if (req.WalletId <= 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Не указан кошелёк" }] };

        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        WalletRetailModelDB walletDb = await context.WalletsRetail
            .Include(x => x.WalletType)
            .FirstAsync(x => x.Id == req.WalletId, cancellationToken: token);

        if (walletDb.WalletType!.IsSystem)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Сочисление на системный кошелёк невозможно" }] };

        req.Version = Guid.NewGuid();
        req.Wallet = null;
        req.PaymentSource = req.PaymentSource?.Trim();
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.DatePayment = req.DatePayment.SetKindUtc();
        req.CreatedAtUTC = DateTime.UtcNow;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);
        PaymentRetailDocumentModelDB docDb = PaymentRetailDocumentModelDB.Build(req);

        await context.PaymentsRetailDocuments.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.Response = docDb.Id;
        res.AddSuccess($"Документ платежа/оплаты создан #{docDb.Id}");

        if (req.InjectToOrderId > 0)
        {
            await context.PaymentsOrdersLinks.AddAsync(new()
            {
                OrderDocumentId = req.InjectToOrderId,
                PaymentDocumentId = docDb.Id,
                AmountPayment = docDb.Amount
            }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь оплаты/платежа #{docDb.Id} с заказом #{req.InjectToOrderId}");
        }

        if (req.StatusPayment == PaymentsRetailStatusesEnum.Paid)
        {
            await context.WalletsRetail
             .Where(x => x.Id == req.WalletId)
             .ExecuteUpdateAsync(set => set
                 .SetProperty(p => p.Balance, p => p.Balance + req.Amount), cancellationToken: token);
        }

        await transaction.CommitAsync(token);
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
    {
        if (req.Amount <= 0)
            return ResponseBaseModel.CreateError("Сумма платежа должна быть больше нуля");

        req.PaymentSource = req.PaymentSource?.Trim();
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.DatePayment = req.DatePayment.SetKindUtc();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        PaymentRetailDocumentModelDB paymentDb = await context.PaymentsRetailDocuments
            .Include(x => x.Wallet!)
            .ThenInclude(x => x.WalletType)
            .FirstAsync(x => x.Id == req.Id, cancellationToken: token);

        if (paymentDb.Version != req.Version)
            return ResponseBaseModel.CreateError("Документ ранее был кем-то изменён. Обновите документ (F5) перед его редактированием.");

        if (req.StatusPayment == paymentDb.StatusPayment && req.StatusPayment == PaymentsRetailStatusesEnum.Paid)
        {
            if (req.WalletId == paymentDb.WalletId)
            {
                decimal _deltaChange = req.Amount - paymentDb.Amount;
                if (_deltaChange < 0 && paymentDb.Wallet!.Balance < -_deltaChange)
                    return ResponseBaseModel.CreateError($"В следствии изменения документа - сумма баланса [wallet:{paymentDb.Wallet.WalletType}] станет отрицательной");

                if (_deltaChange != 0)
                {
                    await context.WalletsRetail
                        .Where(x => x.Id == paymentDb.WalletId)
                        .ExecuteUpdateAsync(set => set
                            .SetProperty(p => p.Balance, p => p.Balance + _deltaChange)
                            .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);
                }
            }
            else if (paymentDb.Wallet!.Balance < paymentDb.Amount)
            {
                return ResponseBaseModel.CreateError($"В следствии изменения документа - сумма баланса [wallet:{paymentDb.Wallet.WalletType}] станет отрицательной");
            }
            else
            {
                await context.WalletsRetail
                    .Where(x => x.Id == req.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + req.Amount), cancellationToken: token);

                await context.WalletsRetail
                    .Where(x => x.Id == paymentDb.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - paymentDb.Amount), cancellationToken: token);
            }
        }
        else if (req.StatusPayment != paymentDb.StatusPayment)
        {
            if (req.StatusPayment == PaymentsRetailStatusesEnum.Paid)
            {
                await context.WalletsRetail
                    .Where(x => x.Id == req.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + req.Amount), cancellationToken: token);
            }
            else if (paymentDb.StatusPayment == PaymentsRetailStatusesEnum.Paid)
            {
                if (paymentDb.Wallet!.Balance < req.Amount)
                    return ResponseBaseModel.CreateError($"В следствии изменения документа - сумма баланса [wallet:{paymentDb.Wallet.WalletType}] станет отрицательной");

                await context.WalletsRetail
                    .Where(x => x.Id == paymentDb.WalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - req.Amount), cancellationToken: token);
            }
        }

        await context.PaymentsRetailDocuments
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.WalletId, req.WalletId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.TypePayment, req.TypePayment)
                .SetProperty(p => p.StatusPayment, req.StatusPayment)
                .SetProperty(p => p.PaymentSource, req.PaymentSource)
                .SetProperty(p => p.DatePayment, req.DatePayment)
                .SetProperty(p => p.Amount, req.Amount)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<PaymentRetailDocumentModelDB> q = context.PaymentsRetailDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

        if (!string.IsNullOrWhiteSpace(req.Payload?.PayerFilterIdentityId))
            q = q.Where(x => context.WalletsRetail.Any(y => y.Id == x.WalletId && y.UserIdentityId == req.Payload.PayerFilterIdentityId));

        if (req.Payload?.TypesFilter is not null && req.Payload.TypesFilter.Count != 0)
            q = q.Where(x => req.Payload.TypesFilter.Contains(x.TypePayment));

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Length != 0)
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.StatusPayment));

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DatePayment >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DatePayment <= req.Payload.End);
        }

        if (req.Payload is not null && req.Payload.ExcludeOrderId > 0)
            q = q.Where(x => !context.PaymentsOrdersLinks.Any(y => y.PaymentDocumentId == x.Id && y.OrderDocumentId == req.Payload.ExcludeOrderId));

        IOrderedQueryable<PaymentRetailDocumentModelDB> oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => q.OrderBy(x => x.DatePayment),
            DirectionsEnum.Down => q.OrderByDescending(x => x.DatePayment),
            _ => q.OrderBy(x => x.Name)
        };

        IQueryable<PaymentRetailDocumentModelDB>? pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq
                .Include(x => x.Wallet!)
                .ThenInclude(x => x.WalletType)
                .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentRetailDocumentModelDB[]>> GetPaymentsDocumentsAsync(GetPaymentsRetailOrdersDocumentsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.Ids is null || req.Ids.Length == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Ids is null || Ids.Length == 0" }] };

        return new()
        {
            Response = await context.PaymentsRetailDocuments
            .Where(x => req.Ids.Contains(x.Id))
            .Include(x => x.Wallet)
            .ToArrayAsync(cancellationToken: token)
        };
    }
    #endregion

    #region Order`s (document`s)
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRetailDocumentAsync(CreateDocumentRetailRequestModel req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
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
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (!string.IsNullOrWhiteSpace(req.ExternalDocumentId) && await context.OrdersRetail.AnyAsync(x => x.Id != req.Id && x.ExternalDocumentId == req.ExternalDocumentId, cancellationToken: token))
            return ResponseBaseModel.CreateError($"Документ [ext #:{req.ExternalDocumentId}] уже существует");

        int res = await context.OrdersRetail
              .Where(x => x.Id == req.Id)
              .ExecuteUpdateAsync(set => set
                  .SetProperty(p => p.Name, req.Name.Trim())
                  .SetProperty(p => p.NumWeekOfYear, req.NumWeekOfYear)
                  .SetProperty(p => p.DateDocument, req.DateDocument.SetKindUtc())
                  .SetProperty(p => p.StatusDocument, req.StatusDocument)
                  .SetProperty(p => p.BuyerIdentityUserId, req.BuyerIdentityUserId)
                  .SetProperty(p => p.Version, Guid.NewGuid())
                  .SetProperty(p => p.Description, req.Description)
                  .SetProperty(p => p.WarehouseId, req.WarehouseId)
                  .SetProperty(p => p.ExternalDocumentId, req.ExternalDocumentId)
                  .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

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

        if (req.UpdateStatuses)
        {
            loggerRepo.LogInformation($"Обновление статусов заказам: {JsonConvert.SerializeObject(req.Ids)}");
            foreach (DocumentRetailModelDB docDb in res.Response)
            {
                await context.OrdersRetail
                    .Where(x => x.Id == docDb.Id)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == docDb.Id).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);
            }
        }

        return res;
    }
    #endregion

    #region Rows for order-document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default)
    {
        if (req.Quantity <= 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Количество должно быть больше нуля" }] };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.Version = Guid.NewGuid();
        req.Order = null;
        req.Nomenclature = null;
        req.Offer = null;

        await context.RowsOrdersRetails.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.OrdersRetail
          .Where(x => x.Id == req.OrderId)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        return new TResponseModel<int>() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowRetailDocumentAsync(RowOfRetailOrderDocumentModelDB req, CancellationToken token = default)
    {
        if (req.Quantity <= 0)
            return ResponseBaseModel.CreateError("Количество должно быть больше нуля");

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.RowsOrdersRetails
          .Where(x => x.Id == req.Id)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Comment, req.Comment)
              .SetProperty(p => p.Quantity, req.Quantity)
              .SetProperty(p => p.Version, Guid.NewGuid())
              .SetProperty(p => p.Amount, req.Amount), cancellationToken: token);

        await context.OrdersRetail
          .Where(x => x.Id == req.OrderId)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfRetailOrderDocumentModelDB>> SelectRowsRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRowsRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
            return new() { Status = new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Payload is null" }] } };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfRetailOrderDocumentModelDB> q = context.RowsOrdersRetails
            .Where(x => x.OrderId == req.Payload.OrderId)
            .AsQueryable();

        IQueryable<RowOfRetailOrderDocumentModelDB>? pq = q
            .OrderBy(x => x.Id)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<RowOfRetailOrderDocumentModelDB> res = await pq.Include(x => x.Offer).ToListAsync(cancellationToken: token);
        foreach (RowOfRetailOrderDocumentModelDB row in res.Where(x => x.Amount <= 0 || x.WeightOffer <= 0))
        {
            if (row.Amount <= 0)
                row.Amount = row.Quantity * row.Offer!.Price;

            if (row.WeightOffer <= 0)
                row.WeightOffer = row.Quantity * row.Offer!.Weight;

            context.Update(row);
            await context.SaveChangesAsync(token);
        }

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
    public async Task<ResponseBaseModel> DeleteRowRetailDocumentAsync(int rowId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.RowsOrdersRetails.Where(x => x.Id == rowId).ExecuteDeleteAsync(cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Строка документа удалена");
    }
    #endregion

    #region Deliveries orders link`s 
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryOrderLinkDocumentAsync(RetailOrderDeliveryLinkModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (await context.OrdersDeliveriesLinks.AnyAsync(x => x.DeliveryDocumentId == req.DeliveryDocumentId && x.OrderDocumentId == req.OrderDocumentId, cancellationToken: token))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Warning, Text = "Документ уже добавлен" }] };

        req.OrderDocument = null;
        req.DeliveryDocument = null;

        await context.OrdersDeliveriesLinks.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryOrderLinkDocumentAsync(RetailOrderDeliveryLinkModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.OrdersDeliveriesLinks
            .Where(x => x.Id == req.Id || (req.OrderDocumentId == x.OrderDocumentId && req.DeliveryDocumentId == x.DeliveryDocumentId))
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.WeightShipping, req.WeightShipping), cancellationToken: token);
        await context.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailOrderDeliveryLinkModelDB>> SelectDeliveriesOrdersLinksDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RetailOrderDeliveryLinkModelDB> q = context.OrdersDeliveriesLinks.AsQueryable();

        bool
            forOrders = req.Payload?.OrdersIds is not null && req.Payload.OrdersIds.Length != 0,
            forDeliveries = req.Payload?.DeliveriesIds is not null && req.Payload.DeliveriesIds.Length != 0;

        if (forOrders)
            q = q.Where(x => req.Payload!.OrdersIds!.Contains(x.OrderDocumentId));

        if (forDeliveries)
            q = q.Where(x => req.Payload!.DeliveriesIds!.Contains(x.DeliveryDocumentId));

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => context.OrdersRetail.Any(y => x.OrderDocumentId == y.Id && y.ExternalDocumentId != null && y.ExternalDocumentId.Contains(req.FindQuery)));

        IQueryable<RetailOrderDeliveryLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.DeliveryDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<RetailOrderDeliveryLinkModelDB> res = (forOrders && forDeliveries) || (!forOrders && !forDeliveries)
                            ? await pq.Include(x => x.DeliveryDocument).Include(x => x.OrderDocument).ToListAsync(cancellationToken: token)
                            : forOrders
                                ? await pq.Include(x => x.DeliveryDocument!).ThenInclude(x => x.Rows!).ThenInclude(x => x.Offer).ToListAsync(cancellationToken: token)
                                : await pq.Include(x => x.OrderDocument!).ThenInclude(x => x.Rows!).ThenInclude(x => x.Offer).ToListAsync(cancellationToken: token);

        if (forOrders != forDeliveries)
            foreach (RetailOrderDeliveryLinkModelDB row in res.Where(x => x.WeightShipping <= 0))
            {
                if (forOrders)
                {
                    row.WeightShipping = row.DeliveryDocument?.Rows is null || row.DeliveryDocument.Rows.Count == 0
                        ? 0
                        : row.DeliveryDocument.Rows.Sum(x => x.WeightOffer);
                }
                else if (forDeliveries)
                {
                    row.WeightShipping = row.OrderDocument?.Rows is null || row.OrderDocument.Rows.Count == 0
                        ? 0
                        : row.OrderDocument.Rows.Sum(x => x.WeightOffer);
                }

                if (row.WeightShipping != 0)
                {
                    context.Update(row);
                    await context.SaveChangesAsync(token);
                }
            }

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
    public async Task<TResponseModel<decimal>> TotalWeightOrdersDocumentsLinksAsync(TotalWeightDeliveriesOrdersLinksDocumentsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        if (req.DeliveryDocumentId <= 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Не указан документ доставки" }] };

        if (!await context.OrdersDeliveriesLinks.AnyAsync(x => x.DeliveryDocumentId == req.DeliveryDocumentId, cancellationToken: token))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Info, Text = "Документ доставки без заказов" }] };

        return new() { Response = await context.OrdersDeliveriesLinks.Where(x => x.DeliveryDocumentId == req.DeliveryDocumentId).SumAsync(x => x.WeightShipping, cancellationToken: token) };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteDeliveryOrderLinkDocumentAsync(DeleteDeliveryOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int res = await context.OrdersDeliveriesLinks
             .Where(x => x.Id == req.OrderDeliveryLinkId || (x.OrderDocumentId == req.OrderId && x.DeliveryDocumentId == req.DeliveryId))
             .ExecuteDeleteAsync(cancellationToken: token);

        return res == 0
            ? ResponseBaseModel.CreateInfo("Объект уже удалён")
            : ResponseBaseModel.CreateSuccess("Удалено");
    }
    #endregion

    #region Payments orders link`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentOrderLinkDocumentAsync(PaymentOrderRetailLinkModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.OrderDocument = null;
        req.PaymentDocument = null;
        req.Name = req.Name?.Trim();

        await context.PaymentsOrdersLinks.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentOrderLinkDocumentAsync(PaymentOrderRetailLinkModelDB req, CancellationToken token = default)
    {
        req.Name = req.Name?.Trim();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.PaymentsOrdersLinks
            .Where(x => x.Id == req.Id || (req.OrderDocumentId == x.OrderDocumentId && req.PaymentDocumentId == x.PaymentDocumentId))
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.AmountPayment, req.AmountPayment), cancellationToken: token);
        await context.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentOrderRetailLinkModelDB>> SelectPaymentsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<PaymentOrderRetailLinkModelDB> q = context.PaymentsOrdersLinks.AsQueryable();

        bool
            forOrders = req.Payload?.OrdersIds is not null && req.Payload.OrdersIds.Length != 0,
            forPayments = req.Payload?.PaymentsIds is not null && req.Payload.PaymentsIds.Length != 0;

        if (forOrders)
            q = q.Where(x => req.Payload!.OrdersIds!.Contains(x.OrderDocumentId));

        if (forPayments)
            q = q.Where(x => req.Payload!.PaymentsIds!.Contains(x.PaymentDocumentId));

        IQueryable<PaymentOrderRetailLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.PaymentDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<PaymentOrderRetailLinkModelDB> res = (forOrders && forPayments) || (!forOrders && !forPayments)
                            ? await pq.Include(x => x.PaymentDocument).Include(x => x.OrderDocument).ToListAsync(cancellationToken: token)
                            : forOrders
                                ? await pq.Include(x => x.PaymentDocument!).ThenInclude(x => x.Wallet).ToListAsync(cancellationToken: token)
                                : await pq.Include(x => x.OrderDocument!).ThenInclude(x => x.Rows!).ThenInclude(x => x.Offer).ToListAsync(cancellationToken: token);

        if (forOrders != forPayments)
            foreach (PaymentOrderRetailLinkModelDB row in res.Where(x => x.AmountPayment <= 0))
            {
                if (forOrders)
                {
                    row.AmountPayment = row.PaymentDocument is null
                        ? 0
                        : row.PaymentDocument.Amount;
                }
                else if (forPayments)
                {
                    row.AmountPayment = row.OrderDocument?.Rows is null || row.OrderDocument.Rows.Count == 0
                        ? 0
                        : row.OrderDocument.Rows.Sum(x => x.Amount);
                }

                if (row.AmountPayment != 0)
                {
                    context.Update(row);
                    await context.SaveChangesAsync(token);
                }
            }

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
    public async Task<ResponseBaseModel> DeletePaymentOrderLinkDocumentAsync(DeletePaymentOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int res = await context.PaymentsOrdersLinks
             .Where(x => x.Id == req.OrderPaymentLinkId || (x.OrderDocumentId == req.OrderId && x.PaymentDocumentId == req.PaymentId))
             .ExecuteDeleteAsync(cancellationToken: token);

        return res == 0
            ? ResponseBaseModel.CreateInfo("Объект уже удалён")
            : ResponseBaseModel.CreateSuccess("Удалено");
    }
    #endregion

    #region Statuses (of order`s document)
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.DateOperation = req.DateOperation.SetKindUtc();
        req.OrderDocument = null;
        req.Name = req.Name.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await context.OrdersStatusesRetails.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        loggerRepo.LogInformation($"Для заказа (розница) #{req.OrderDocumentId} добавлен статус: [{req.DateOperation}] {req.StatusDocument}");

        await context.OrdersRetail
            .Where(x => x.Id == req.OrderDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        await transaction.CommitAsync(token);

        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        req.Name = req.Name.Trim();
        await context.OrdersStatusesRetails
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.StatusDocument, req.StatusDocument)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        loggerRepo.LogInformation($"Для заказа (розница) #{req.OrderDocumentId} обновлён статус: [{req.DateOperation}] {req.StatusDocument}");

        await context.OrdersRetail
            .Where(x => x.Id == req.OrderDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderStatusRetailDocumentModelDB>> SelectOrderDocumentStatusesAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            string msg = "req.Payload is null";
            loggerRepo.LogError(msg);
            return new()
            {
                Status = new()
                {
                    Messages = [new()
                    {
                        TypeMessage = MessagesTypesEnum.Error,
                        Text = msg
                    }]
                }
            };
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrderStatusRetailDocumentModelDB> q = context.OrdersStatusesRetails
            .Where(x => x.OrderDocumentId == req.Payload.OrderDocumentId).AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

        IQueryable<OrderStatusRetailDocumentModelDB>? pq = q
            .OrderBy(x => x.DateOperation)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq.ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteOrderStatusDocumentAsync(int statusId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrderStatusRetailDocumentModelDB> q = context.OrdersStatusesRetails.Where(x => x.Id == statusId);
        int deliveryDocumentId = await q.Select(x => x.OrderDocumentId).FirstAsync(cancellationToken: token);

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await q.ExecuteDeleteAsync(cancellationToken: token);

        loggerRepo.LogInformation($"Для заказа (розница) #{deliveryDocumentId} удалён статус #{statusId}");

        await context.OrdersRetail
            .Where(x => x.Id == deliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == deliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        await transaction.CommitAsync(cancellationToken: token);
        return ResponseBaseModel.CreateSuccess("Строка-статус успешно удалена");
    }
    #endregion

    #region Conversion`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentRetailAsync(CreateWalletConversionRetailDocumentRequestModel req, CancellationToken token = default)
    {
        if (req.ToWalletId < 1 || req.ToWalletId < 1)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите кошельки списания и зачисления!"
                }]
            };

        if (req.ToWalletSum <= 0 || req.FromWalletSum <= 0)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите сумму списания и зачисления!"
                }]
            };

        if (req.ToWalletId == req.FromWalletId)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Счёт списания не может совпадать со счётом зачисления"
                }]
            };
        TResponseModel<int> res = new();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        WalletRetailModelDB[] walletsDb = await context.WalletsRetail
            .Where(x => x.Id == req.FromWalletId || x.Id == req.ToWalletId)
            .Include(x => x.WalletType)
            .ToArrayAsync(cancellationToken: token);

        WalletRetailModelDB
            walletSender = walletsDb.First(x => x.Id == req.FromWalletId),
            walletRecipient = walletsDb.First(x => x.Id == req.ToWalletId);

        if (!walletSender.WalletType!.IsSystem && !walletSender.WalletType!.IgnoreBalanceChanges && walletSender.Balance - req.FromWalletSum < 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Баланс не может стать отрицательным в следствии списания" }] };

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!walletSender.WalletType.IgnoreBalanceChanges)
            await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - req.FromWalletSum), cancellationToken: token);

        if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
            await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Balance, p => p.Balance + req.ToWalletSum), cancellationToken: token);

        req.Name = req.Name.Trim();
        req.Version = Guid.NewGuid();
        req.ToWallet = null;
        req.FromWallet = null;
        req.CreatedAtUTC = DateTime.UtcNow;
        req.DateDocument = req.DateDocument.SetKindUtc();

        WalletConversionRetailDocumentModelDB docDb = WalletConversionRetailDocumentModelDB.Build(req);

        await context.ConversionsDocumentsWalletsRetail.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.AddSuccess($"Документ перевода/конвертации создан #{docDb.Id}");

        if (req.InjectToOrderId > 0)
        {
            await context.ConversionsOrdersLinksRetail.AddAsync(new()
            {
                ConversionDocumentId = docDb.Id,
                OrderDocumentId = req.InjectToOrderId,
                AmountPayment = req.ToWalletSum,
            }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь документа перевода/конвертации #{docDb.Id} с заказом #{req.InjectToOrderId}");
        }

        await transaction.CommitAsync(token);
        return new TResponseModel<int>() { Response = docDb.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionDocumentRetailAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
    {
        if (req.ToWalletId < 1 || req.ToWalletId < 1)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите кошельки списания и зачисления!"
                }]
            };

        if (req.ToWalletSum <= 0 || req.FromWalletSum <= 0)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Укажите сумму списания и зачисления!"
                }]
            };

        if (req.ToWalletId == req.FromWalletId)
            return new()
            {
                Messages = [new()
                {
                    TypeMessage = MessagesTypesEnum.Error,
                    Text = "Счёт списания не может совпадать со счётом зачисления"
                }]
            };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        WalletConversionRetailDocumentModelDB _conversionDocDb = await context.ConversionsDocumentsWalletsRetail.FirstAsync(x => x.Id == req.Id, cancellationToken: token);
        int[] _walletsIds = [req.FromWalletId, req.ToWalletId, _conversionDocDb.FromWalletId, _conversionDocDb.ToWalletId];

        WalletRetailModelDB[] walletsDb = await context.WalletsRetail
           .Where(x => _walletsIds.Contains(x.Id))
           .Include(x => x.WalletType)
           .ToArrayAsync(cancellationToken: token);

        WalletRetailModelDB
            walletSenderDb = walletsDb.First(x => x.Id == _conversionDocDb.FromWalletId),
            walletRecipientDb = walletsDb.First(x => x.Id == _conversionDocDb.ToWalletId),
            walletSender = walletsDb.First(x => x.Id == req.FromWalletId),
            walletRecipient = walletsDb.First(x => x.Id == req.ToWalletId);

        if (_conversionDocDb.Version != req.Version)
            return ResponseBaseModel.CreateError("Документ уже кем-то изменён. Обновите страницу с документом и повторите попытку");

        decimal
            _deltaSender = req.FromWalletSum - _conversionDocDb.FromWalletSum,
            _deltaRecipient = req.ToWalletSum - _conversionDocDb.ToWalletSum;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (req.FromWalletId == _conversionDocDb.FromWalletId)
        {
            if (!walletSender.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - _deltaSender), cancellationToken: token);
        }
        else
        {
            if (!walletSenderDb.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == _conversionDocDb.FromWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + _conversionDocDb.FromWalletSum), cancellationToken: token);

            if (!walletSender.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - req.FromWalletSum), cancellationToken: token);
        }

        if (req.ToWalletId == _conversionDocDb.ToWalletId)
        {
            if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance + _deltaRecipient), cancellationToken: token);
        }
        else
        {
            if (!walletRecipientDb.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == _conversionDocDb.ToWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance - _conversionDocDb.ToWalletSum), cancellationToken: token);

            if (!walletRecipient.WalletType!.IgnoreBalanceChanges)
                await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
                    .ExecuteUpdateAsync(set => set
                        .SetProperty(p => p.Balance, p => p.Balance + req.ToWalletSum), cancellationToken: token);
        }

        await context.ConversionsDocumentsWalletsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name.Trim())
                .SetProperty(p => p.FromWalletId, req.FromWalletId)
                .SetProperty(p => p.FromWalletSum, req.FromWalletSum)
                .SetProperty(p => p.ToWalletId, req.ToWalletId)
                .SetProperty(p => p.ToWalletSum, req.ToWalletSum)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await transaction.CommitAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/> 
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsRetailAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default)
    {
        if (req.Ids.Length == 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка запроса: Ids.Length == 0" }] };

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        return new()
        {
            Response = await context.ConversionsDocumentsWalletsRetail
                .Where(x => req.Ids.Contains(x.Id))
                .Include(x => x.ToWallet)
                .Include(x => x.FromWallet)
                .ToArrayAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsRetailAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletConversionRetailDocumentModelDB> q = context.ConversionsDocumentsWalletsRetail.AsQueryable();

        if (req.Payload?.IncludeDisabled != true)
            q = q.Where(x => !x.IsDisabled);

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

        string[]
            sendersUserFilter = req.Payload?.SendersUserFilter ?? [],
            recipientsUserFilter = req.Payload?.RecipientsUserFilter ?? [];

        q = from doc in q
            join sender in context.WalletsRetail on doc.FromWalletId equals sender.Id
            join recipient in context.WalletsRetail on doc.ToWalletId equals recipient.Id

            where sendersUserFilter.Length == 0 || sendersUserFilter.Contains(sender.UserIdentityId) || recipientsUserFilter.Contains(recipient.UserIdentityId)

            select doc;

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DateDocument >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.Payload.End);
        }

        if (req.Payload is not null && req.Payload.ExcludeOrderId > 0)
            q = q.Where(x => !context.ConversionsOrdersLinksRetail.Any(y => y.ConversionDocumentId == x.Id && y.OrderDocumentId == req.Payload.ExcludeOrderId));

        IOrderedQueryable<WalletConversionRetailDocumentModelDB> oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => q.OrderBy(x => x.DateDocument),
            DirectionsEnum.Down => q.OrderByDescending(x => x.DateDocument),
            _ => q.OrderBy(x => x.Name)
        };

        IQueryable<WalletConversionRetailDocumentModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq
                .Include(x => x.FromWallet).ThenInclude(x => x!.WalletType).Include(x => x.Orders)
                .Include(x => x.ToWallet).ThenInclude(x => x!.WalletType).Include(x => x.Orders)
                .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteToggleConversionRetailAsync(int conversionId, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WalletConversionRetailDocumentModelDB> q = context.ConversionsDocumentsWalletsRetail
            .Where(x => x.Id == conversionId);

        WalletConversionRetailDocumentModelDB conversionDb = await q
            .Include(x => x.ToWallet!).ThenInclude(x => x.WalletType)
            .Include(x => x.FromWallet!).ThenInclude(x => x.WalletType)
            .FirstAsync(x => x.Id == conversionId, cancellationToken: token);

        if (conversionDb.ToWalletId < 1 || conversionDb.ToWalletId < 1)
            return ResponseBaseModel.CreateError("Укажите кошельки списания и зачисления!");

        if (conversionDb.ToWalletSum <= 0 || conversionDb.FromWalletSum <= 0)
            return ResponseBaseModel.CreateError("Укажите сумму списания и зачисления!");

        if (conversionDb.ToWalletId == conversionDb.FromWalletId)
            return ResponseBaseModel.CreateError("Счёт списания не может совпадать со счётом зачисления");

        conversionDb.IsDisabled = !conversionDb.IsDisabled;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (!conversionDb.FromWallet!.WalletType!.IsSystem && !conversionDb.FromWallet.WalletType!.IgnoreBalanceChanges && conversionDb.FromWallet.Balance < conversionDb.FromWalletSum)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Баланс не может стать отрицательным в следствии списания" }] };

        await context.WalletsRetail
                .Where(x => x.Id == conversionDb.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, b => b.Balance - conversionDb.FromWalletSum), cancellationToken: token);

        if (!conversionDb.ToWallet!.WalletType!.IgnoreBalanceChanges)
        {
            await context.WalletsRetail
                .Where(x => x.Id == conversionDb.ToWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, b => b.Balance + conversionDb.ToWalletSum), cancellationToken: token);
        }

        int res = await q.ExecuteUpdateAsync(set => set
                     .SetProperty(p => p.IsDisabled, conversionDb.IsDisabled)
                     .SetProperty(p => p.Version, Guid.NewGuid()), cancellationToken: token);

        await transaction.CommitAsync(token);

        return
            ResponseBaseModel
            .CreateSuccess($"Документ: успешно {(conversionDb.IsDisabled ? "выключен" : "включён")}");
    }
    #endregion

    #region Conversions orders link`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionOrderLinkDocumentRetailAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (await context.ConversionsOrdersLinksRetail.AnyAsync(x => x.ConversionDocumentId == req.ConversionDocumentId && x.OrderDocumentId == req.OrderDocumentId, cancellationToken: token))
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Warning, Text = "Документ уже добавлен" }] };

        req.OrderDocument = null;
        req.ConversionDocument = null;

        await context.ConversionsOrdersLinksRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionOrderLinkDocumentRetailAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.ConversionsOrdersLinksRetail
            .Where(x => x.Id == req.Id || (req.OrderDocumentId == x.OrderDocumentId && req.ConversionDocumentId == x.ConversionDocumentId))
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.AmountPayment, req.AmountPayment), cancellationToken: token);

        await context.SaveChangesAsync(token);
        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ConversionOrderRetailLinkModelDB>> SelectConversionsOrdersDocumentsLinksRetailAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<ConversionOrderRetailLinkModelDB> q = context.ConversionsOrdersLinksRetail.AsQueryable();

        bool
            forOrders = req.Payload?.OrdersIds is not null && req.Payload.OrdersIds.Length != 0,
            forConversions = req.Payload?.ConversionsIds is not null && req.Payload.ConversionsIds.Length != 0;

        if (forOrders)
            q = q.Where(x => req.Payload!.OrdersIds!.Contains(x.OrderDocumentId));

        if (forConversions)
            q = q.Where(x => req.Payload!.ConversionsIds!.Contains(x.ConversionDocumentId));

        IQueryable<ConversionOrderRetailLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.ConversionDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ConversionOrderRetailLinkModelDB, DocumentRetailModelDB?>? v = pq
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.FromWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.ToWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.OrderDocument);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ConversionOrderRetailLinkModelDB, DocumentRetailModelDB?> BuildQuery()
        {
            return pq
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.FromWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.ConversionDocument!)
            .ThenInclude(x => x.ToWallet!)
            .ThenInclude(x => x.WalletType)
            .Include(x => x.OrderDocument);
        }

        List<ConversionOrderRetailLinkModelDB> res = await BuildQuery().ToListAsync(cancellationToken: token);

        if (forOrders != forConversions)
            foreach (ConversionOrderRetailLinkModelDB row in res.Where(x => x.AmountPayment <= 0))
            {
                if (forOrders)
                {
                    row.AmountPayment = row.ConversionDocument is null
                        ? 0
                        : row.ConversionDocument.ToWalletSum;
                }
                else if (forConversions)
                {
                    row.AmountPayment = row.OrderDocument?.Rows is null || row.OrderDocument.Rows.Count == 0
                        ? 0
                        : row.OrderDocument.Rows.Sum(x => x.Amount);
                }

                if (row.AmountPayment != 0)
                {
                    context.Update(row);
                    await context.SaveChangesAsync(token);
                }
            }


        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = res,
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteConversionOrderLinkDocumentRetailAsync(DeleteConversionOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        int res = await context.ConversionsOrdersLinksRetail
             .Where(x => x.Id == req.OrderConversionLinkId || (x.OrderDocumentId == req.OrderId && x.ConversionDocumentId == req.ConversionId))
             .ExecuteDeleteAsync(cancellationToken: token);

        return res == 0
            ? ResponseBaseModel.CreateInfo("Объект уже удалён")
            : ResponseBaseModel.CreateSuccess("Удалено");
    }
    #endregion

    #region report`s
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfOrdersReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfOrdersRetailReportRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DocumentRetailModelDB> q = context.OrdersRetail.AsQueryable();

        if (req.Payload?.EqualsSumFilter != true)
            q = context.OrdersRetail
                .Where(x => context.RowsOrdersRetails.Where(y => y.OrderId == x.Id).Sum(y => y.Amount) == (context.PaymentsOrdersLinks.Where(y => y.OrderDocumentId == x.Id && context.PaymentsRetailDocuments.Any(z => z.StatusPayment == PaymentsRetailStatusesEnum.Paid && z.Id == y.PaymentDocumentId)).Sum(y => y.AmountPayment) + context.ConversionsOrdersLinksRetail.Where(y => y.OrderDocumentId == x.Id && context.ConversionsDocumentsWalletsRetail.Any(z => z.Id == y.ConversionDocumentId && !z.IsDisabled)).Sum(y => y.AmountPayment)))
                .AsQueryable();

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Count != 0)
        {
            bool _unsetChecked = req.Payload.StatusesFilter.Contains(null);
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.StatusDocument) || (_unsetChecked && x.StatusDocument == 0));
        }

        if (req.Payload is not null && req.Payload.NumWeekOfYear > 0)
            q = q.Where(x => x.NumWeekOfYear == req.Payload.NumWeekOfYear);

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DateDocument >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.Payload.End);
        }

        IQueryable<RowOfRetailOrderDocumentModelDB>? qr = context.RowsOrdersRetails
           .Where(x => q.Any(y => y.Id == x.OrderId))
           .AsQueryable();

        var _fq = from p in qr
                  group p by p.OfferId
                  into g
                  select new { OfferId = g.Key, Amount = g.Sum(x => x.Amount), Counter = g.Sum(x => x.Quantity) };

        var oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => _fq.OrderBy(x => x.Amount),
            DirectionsEnum.Down => _fq.OrderByDescending(x => x.Amount),
            _ => _fq.OrderBy(x => x.OfferId)
        };

        var pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        var res = await pq.ToListAsync(cancellationToken: token);
        if (res.Count == 0)
            return new()
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                SortingDirection = req.SortingDirection,
                SortBy = req.SortBy,
                TotalRowsCount = 0,
                Response = []
            };

        int[] offersIds = [.. res.Select(x => x.OfferId)];
        OfferModelDB[] offersDb = await context.Offers.Where(x => offersIds.Contains(x.Id)).ToArrayAsync(cancellationToken: token);

        OffersRetailReportRowModel getObject(decimal offerId, decimal amountSum, decimal countSum)
        {
            return new()
            {
                Sum = amountSum,
                Count = countSum,
                Offer = offersDb.First(x => x.Id == offerId)
            };
        }

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await _fq.CountAsync(cancellationToken: token),
            Response = [.. res.Select(x => getObject(x.OfferId, x.Amount, x.Counter))],
        };
    }

    /// <inheritdoc/>
    public async Task<MainReportResponseModel> GetMainReportAsync(MainReportRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<DocumentRetailModelDB> q = context.OrdersRetail
            .Where(x => x.StatusDocument == StatusesDocumentsEnum.Done)
            .AsQueryable();

        if (req.NumWeekOfYear > 0)
            q = q.Where(x => x.NumWeekOfYear == req.NumWeekOfYear);

        if (req.Start is not null && req.Start != default)
            q = q.Where(x => x.DateDocument >= req.Start.SetKindUtc());

        if (req.End is not null && req.End != default)
        {
            req.End = req.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.End);
        }

        IQueryable<PaymentOrderRetailLinkModelDB> qpo = context.PaymentsOrdersLinks
            .Where(x => x.PaymentDocument!.StatusPayment == PaymentsRetailStatusesEnum.Paid && q.Any(y => y.Id == x.OrderDocumentId));

        IQueryable<ConversionOrderRetailLinkModelDB> qco = context.ConversionsOrdersLinksRetail
            .Where(x => !x.ConversionDocument!.IsDisabled && q.Any(y => y.Id == x.OrderDocumentId));

        return new()
        {
            DoneOrdersCount = await q.CountAsync(cancellationToken: token),
            DoneOrdersSumAmount = await context.RowsOrdersRetails.Where(x => q.Any(y => y.Id == x.OrderId)).SumAsync(x => x.Amount, cancellationToken: token),

            PaidOnSitePaymentsSumAmount = await qpo.Where(x => x.PaymentDocument!.TypePayment == PaymentsRetailTypesEnum.OnSite).SumAsync(x => x.AmountPayment, cancellationToken: token),
            PaidOnSitePaymentsCount = await qpo.Where(x => x.PaymentDocument!.TypePayment == PaymentsRetailTypesEnum.OnSite).CountAsync(token),

            PaidNoSitePaymentsSumAmount = await qpo.Where(x => x.PaymentDocument!.TypePayment != PaymentsRetailTypesEnum.OnSite).SumAsync(x => x.AmountPayment, cancellationToken: token),
            PaidNoSitePaymentsCount = await qpo.Where(x => x.PaymentDocument!.TypePayment != PaymentsRetailTypesEnum.OnSite).CountAsync(cancellationToken: token),

            ConversionsSumAmount = await qco.SumAsync(x => x.AmountPayment, cancellationToken: token),
            ConversionsCount = await qco.CountAsync(token),
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailReportRowModel>> FinancialsReportRetailAsync(TPaginationRequestStandardModel<SelectPaymentsRetailReportRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        //IQueryable<PaymentOrderRetailLinkModelDB> qp = context.PaymentsOrdersLinks
        //    .Where(x => x.PaymentDocument!.StatusPayment == PaymentsRetailStatusesEnum.Paid)
        //    .Where(x => x.OrderDocument!.StatusDocument == StatusesDocumentsEnum.Done);

        //IQueryable<ConversionOrderRetailLinkModelDB> qc = context.ConversionsOrdersLinksRetail
        //    .Where(x => !x.ConversionDocument!.IsDisabled)
        //    .Where(x => x.OrderDocument!.StatusDocument == StatusesDocumentsEnum.Done);

        //if (req.Payload?.FilterIdentityIds is not null && req.Payload.FilterIdentityIds.Length != 0)
        //{
        //    qp = qp.Where(x => context.WalletsRetail.Any(y => y.Id == x.PaymentDocument!.WalletId && req.Payload.FilterIdentityIds.Contains(y.UserIdentityId)));

        //    qc = from doc in qc
        //         join sender in context.WalletsRetail on doc.ConversionDocument!.FromWalletId equals sender.Id
        //         join recipient in context.WalletsRetail on doc.ConversionDocument!.ToWalletId equals recipient.Id

        //         where req.Payload.FilterIdentityIds.Length == 0 || req.Payload.FilterIdentityIds.Contains(sender.UserIdentityId) || req.Payload.FilterIdentityIds.Contains(recipient.UserIdentityId)

        //         select doc;
        //}

        //bool conversionCheck =
        //    req.Payload?.TypesFilter is null ||
        //    req.Payload.TypesFilter.Count == 0 ||
        //    req.Payload.TypesFilter.Contains(null);

        //bool paymentsCheck =
        //    req.Payload?.TypesFilter is null ||
        //    req.Payload.TypesFilter.Count == 0 ||
        //    req.Payload.TypesFilter.Any(x => x is not null);

        //if (req.Payload?.TypesFilter is not null && req.Payload.TypesFilter.Any(x => x is not null))
        //{
        //    req.Payload.TypesFilter.RemoveAll(x => x is null);
        //    qp = qp.Where(x => req.Payload.TypesFilter.Contains(x.PaymentDocument!.TypePayment));
        //}

        //if (req.Payload is not null && req.Payload.NumWeekOfYear > 0)
        //{
        //    qp = qp.Where(x => x.OrderDocument!.NumWeekOfYear == req.Payload.NumWeekOfYear);
        //    qc = qc.Where(x => x.OrderDocument!.NumWeekOfYear == req.Payload.NumWeekOfYear);
        //}

        //if (req.Payload?.Start is not null && req.Payload.Start != default)
        //{
        //    qp = qp.Where(x => x.PaymentDocument!.DatePayment >= req.Payload.Start.SetKindUtc());
        //    qc = qc.Where(x => x.ConversionDocument!.DateDocument >= req.Payload.Start.SetKindUtc());
        //}

        //if (req.Payload?.End is not null && req.Payload.End != default)
        //{
        //    req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
        //    qp = qp.Where(x => x.PaymentDocument!.DatePayment <= req.Payload.End);
        //    qc = qc.Where(x => x.ConversionDocument!.DateDocument <= req.Payload.End);
        //}

        //var _qp1 = qp.Select(x => new { x.PaymentDocument!.WalletId, Amount = x.AmountPayment });
        //var _qp2 = qc.Select(x => new { WalletId = x.ConversionDocument!.FromWalletId, Amount = -x.ConversionDocument.FromWalletSum });
        //var _qp3 = qc.Select(x => new { WalletId = x.ConversionDocument!.ToWalletId, Amount = x.ConversionDocument.ToWalletSum });

        //var unionQuery = conversionCheck ? _qp1.Union(_qp2).Union(_qp3) : _qp1;
        //if (!paymentsCheck)
        //    unionQuery = _qp2.Union(_qp3);

        //var _fq = from p in unionQuery
        //          group p by p.WalletId
        //          into g
        //          select new { WalletId = g.Key, Amount = g.Sum(x => x.Amount) };

        //var _oq = req.SortingDirection == DirectionsEnum.Up
        //    ? _fq.OrderBy(x => x.Amount)
        //    : _fq.OrderByDescending(x => x.Amount);

        //var pq = _oq
        //    .Skip(req.PageNum * req.PageSize)
        //    .Take(req.PageSize);

        //var res = await pq.ToListAsync(cancellationToken: token);

        //int[] _walletsIds = [.. res.Select(x => x.WalletId).Distinct()];
        //WalletRetailModelDB[] getWalletsDb = await context.WalletsRetail
        //    .Where(x => _walletsIds.Contains(x.Id))
        //    .Include(x => x.WalletType)
        //    .ToArrayAsync(cancellationToken: token);

        //string[] usersIds = [.. getWalletsDb.Select(x => x.UserIdentityId).Distinct()];
        //TResponseModel<UserInfoModel[]> getUsers = await identityRepo.GetUsersOfIdentityAsync(usersIds, token);

        //WalletRetailReportRowModel getObject(int walletId, decimal amount)
        //{
        //    WalletRetailModelDB _w = getWalletsDb.First(y => y.Id == walletId);
        //    return new()
        //    {
        //        Amount = amount,
        //        Wallet = _w,
        //        User = getUsers.Response!.First(x => x.UserId == _w.UserIdentityId)
        //    };
        //}

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            //TotalRowsCount = await _fq.CountAsync(cancellationToken: token),
            //Response = [.. res.Select(x => getObject(x.WalletId, x.Amount))]
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OffersRetailReportRowModel>> OffersOfDeliveriesReportRetailAsync(TPaginationRequestStandardModel<SelectOffersOfDeliveriesRetailReportRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryDocumentRetailModelDB> q = context.DeliveryDocumentsRetail.AsQueryable();

        if (req.Payload?.EqualsSumFilter != true)
            q = context.DeliveryDocumentsRetail
                .Where(x => context.RowsDeliveryDocumentsRetail.Where(y => y.DocumentId == x.Id).Sum(y => y.WeightOffer) == (context.OrdersDeliveriesLinks.Where(y => y.OrderDocumentId == x.Id && context.DeliveryDocumentsRetail.Any(z => z.DeliveryStatus == DeliveryStatusesEnum.Delivered && z.Id == y.DeliveryDocumentId)).Sum(y => y.WeightShipping)))
                .AsQueryable();

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Count != 0)
        {
            bool _unsetChecked = req.Payload.StatusesFilter.Contains(null);
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.DeliveryStatus) || (_unsetChecked && x.DeliveryStatus == 0));
        }

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.CreatedAtUTC >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.CreatedAtUTC <= req.Payload.End);
        }

        IQueryable<RowOfDeliveryRetailDocumentModelDB> qr = context.RowsDeliveryDocumentsRetail.Where(x => q.Any(y => y.Id == x.DocumentId)).AsQueryable();


        var _fq = from p in qr
                  group p by p.OfferId
                  into g
                  select new { OfferId = g.Key, Weight = g.Sum(x => x.WeightOffer), Counter = g.Sum(x => x.Quantity) };

        var oq = req.SortingDirection switch
        {
            DirectionsEnum.Up => _fq.OrderBy(x => x.Weight),
            DirectionsEnum.Down => _fq.OrderByDescending(x => x.Weight),
            _ => _fq.OrderBy(x => x.OfferId)
        };

        var pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        var res = await pq.ToListAsync(cancellationToken: token);
        if (res.Count == 0)
            return new()
            {
                PageNum = req.PageNum,
                PageSize = req.PageSize,
                SortingDirection = req.SortingDirection,
                SortBy = req.SortBy,
                TotalRowsCount = 0,
                Response = []
            };

        int[] offersIds = [.. res.Select(x => x.OfferId)];
        OfferModelDB[] offersDb = await context.Offers.Where(x => offersIds.Contains(x.Id)).ToArrayAsync(cancellationToken: token);

        OffersRetailReportRowModel getObject(decimal offerId, decimal weightSum, decimal countSum)
        {
            return new()
            {
                Sum = weightSum,
                Count = countSum,
                Offer = offersDb.First(x => x.Id == offerId)
            };
        }

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await _fq.CountAsync(cancellationToken: token),
            Response = [.. res.Select(x => getObject(x.OfferId, x.Weight, x.Counter))],
        };
    }

    /// <inheritdoc/>
    public async Task<PeriodBaseModel> AboutPeriodAsync(object? req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<DateTime>
            q1 = context.OrdersRetail.Select(x => x.DateDocument),
            q2 = context.PaymentsRetailDocuments.Select(x => x.DatePayment),
            q3 = context.DeliveryDocumentsRetail.Select(x => x.CreatedAtUTC),
            q4 = context.ConversionsDocumentsWalletsRetail.Select(x => x.DateDocument);

        IQueryable<DateTime> q = q1.Union(q2).Union(q3).Union(q4);
        if (!await q.AnyAsync(cancellationToken: token))
            return new();

        return new()
        {
            Start = await q.MinAsync(cancellationToken: token),
            End = await q.MaxAsync(cancellationToken: token),
        };
    }
    #endregion

    #region static
    /*static Stylesheet GenerateExcelStyleSheet()
    {
        return new Stylesheet(
            new Fonts(
                new Font(                                                               // Стиль под номером 0 - Шрифт по умолчанию.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Calibri" }),
                new Font(                                                               // Стиль под номером 1 - Жирный шрифт Times New Roman.
                    new Bold(),
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Times New Roman" }),
                new Font(                                                               // Стиль под номером 2 - Обычный шрифт Times New Roman.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Times New Roman" }),
                new Font(                                                               // Стиль под номером 3 - Шрифт Times New Roman размером 14.
                    new FontSize() { Val = 14 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
        new FontName() { Val = "Times New Roman" })
        ),
        new Fills(
                new Fill(                                                           // Стиль под номером 0 - Заполнение ячейки по умолчанию.
        new PatternFill() { PatternType = PatternValues.None }),
                new Fill(                                                           // Стиль под номером 1 - Заполнение ячейки серым цветом
                    new PatternFill(
                        new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFAAAAAA" } }
        )
                    { PatternType = PatternValues.Solid }),
        new Fill(                                                           // Стиль под номером 2 - Заполнение ячейки красным.
                    new PatternFill(
                        new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFEFEFEF" } }
                    )
                    { PatternType = PatternValues.Solid })
            )
            ,
            new Borders(
                new Border(                                                         // Стиль под номером 0 - Грани.
                    new LeftBorder(),
                    new RightBorder(),
                    new TopBorder(),
                    new BottomBorder(),
                    new DiagonalBorder()),
                new Border(                                                         // Стиль под номером 1 - Грани
                    new LeftBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Medium },
                    new RightBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Medium },
                    new TopBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Medium },
                    new BottomBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Medium },
                    new DiagonalBorder()),
                new Border(                                                         // Стиль под номером 2 - Грани.
                    new LeftBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Thin },
                    new RightBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Thin },
                    new TopBorder(
                        new Color() { Auto = true }
                    )
                    { Style = BorderStyleValues.Thin },
                    new BottomBorder(
                        new Color() { Indexed = (UInt32Value)64U }
                    )
                    { Style = BorderStyleValues.Thin },
                    new DiagonalBorder())
            ),
            new CellFormats(
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Стиль под номером 0 - The default cell style.  (по умолчанию)
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 1, FillId = 2, BorderId = 1, ApplyFont = true },       // Стиль под номером 1 - Bold 
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 2, ApplyFont = true },       // Стиль под номером 2 - REgular
                new CellFormat() { FontId = 3, FillId = 0, BorderId = 2, ApplyFont = true, NumberFormatId = 4 },       // Стиль под номером 3 - Times Roman
                new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Стиль под номером 4 - Yellow Fill
                new CellFormat(                                                                   // Стиль под номером 5 - Alignment
                    new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                )
                { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true },      // Стиль под номером 6 - Border
                new CellFormat(new Alignment() { Horizontal = HorizontalAlignmentValues.Right, Vertical = VerticalAlignmentValues.Center, WrapText = true }) { FontId = 2, FillId = 0, BorderId = 2, ApplyFont = true, NumberFormatId = 4 }       // Стиль под номером 7 - Задает числовой формат полю.
            )
        );
    }

    static void InsertExcelCell(Row row, int cell_num, string val, CellValues type, uint styleIndex)
    {
        Cell? refCell = null;
        Cell newCell = new() { CellReference = cell_num.ToString() + ":" + row.RowIndex?.ToString(), StyleIndex = styleIndex };
        row.InsertBefore(newCell, refCell);

        newCell.CellValue = new CellValue(val);
        newCell.DataType = new EnumValue<CellValues>(type);
    }

    async Task<byte[]> SaveDeliveriesJournalAsExcel(SelectDeliveryDocumentsRetailRequestModel req, List<DeliveryDocumentRetailModelDB> journalDb, CommerceContext context, CancellationToken token = default)
    {
        string[] usersList = [.. journalDb.Select(x => x.RecipientIdentityUserId).Distinct()];
        TResponseModel<UserInfoModel[]> users = await identityRepo.GetUsersOfIdentityAsync(usersList, token);
        UserInfoModel[] usersDb = users.Response ?? throw new("users for deliveries journal - IS NULL");

        using MemoryStream HTMLStream = new();
        //WorkbookPart? wBookPart = null;

        //using SpreadsheetDocument spreadsheetDoc = SpreadsheetDocument.Create(HTMLStream, SpreadsheetDocumentType.Workbook);

        //wBookPart = spreadsheetDoc.AddWorkbookPart();
        //wBookPart.Workbook = new Workbook();
        //uint sheetId = 1;
        //WorkbookPart workbookPart = spreadsheetDoc.WorkbookPart ?? spreadsheetDoc.AddWorkbookPart();

        //WorkbookStylesPart wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();

        //wbsp.Stylesheet = GenerateExcelStyleSheet();
        //wbsp.Stylesheet.Save();

        //workbookPart.Workbook.Sheets = new Sheets();

        //WorksheetPart wSheetPart;

        //Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());
        //foreach (DeliveryDocumentRetailModelDB table in journalDb)
        //{
        //    wSheetPart = wBookPart.AddNewPart<WorksheetPart>();
        //    Sheet sheet = new() { Id = workbookPart.GetIdOfPart(wSheetPart), SheetId = sheetId, Name = $"№{table.Id} {table.Name}" };//
        //    sheets.Append(sheet);

        //    SheetData sheetData = new();
        //    wSheetPart.Worksheet = new Worksheet(sheetData);

        //    Columns lstColumns = wSheetPart.Worksheet.GetFirstChild<Columns>()!;
        //    bool needToInsertColumns = false;
        //    if (lstColumns == null)
        //    {
        //        lstColumns = new Columns();
        //        needToInsertColumns = true;
        //    }

        //    lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 100, CustomWidth = true, BestFit = true, });
        //    lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 8, CustomWidth = true, BestFit = true, });
        //    lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 8, CustomWidth = true, BestFit = true, });
        //    lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 15, CustomWidth = true, BestFit = true, });
        //    lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 15, CustomWidth = true, BestFit = true, });

        //    if (needToInsertColumns)
        //        wSheetPart.Worksheet.InsertAt(lstColumns, 0);

        //    Row topRow = new() { RowIndex = 2 };
        //    InsertExcelCell(topRow, 1, $"Адрес доставки: {table.KladrTitle} {table.AddressUserComment}", CellValues.String, 0);
        //    sheetData!.Append(topRow);

        //    topRow = new() { RowIndex = 3 };
        //    InsertExcelCell(topRow, 1, $"Получатель: {usersDb.First(x=>x.UserId == table.RecipientIdentityUserId).ToString()}", CellValues.String, 0);
        //    sheetData!.Append(topRow);

        //    Row headerRow = new() { RowIndex = 5 };
        //    InsertExcelCell(headerRow, 1, "Наименование", CellValues.String, 1);
        //    InsertExcelCell(headerRow, 2, "Цена", CellValues.String, 1);
        //    InsertExcelCell(headerRow, 3, "Кол-во", CellValues.String, 1);
        //    InsertExcelCell(headerRow, 4, "Вес", CellValues.String, 1);
        //    InsertExcelCell(headerRow, 5, "Сумма", CellValues.String, 1);
        //    sheetData.AppendChild(headerRow);

        //    uint row_index = 5;
        //    foreach (RowOfDeliveryRetailDocumentModelDB dr in table.Rows!)
        //    {
        //        Row row = new() { RowIndex = row_index++ };
        //        InsertExcelCell(row, 1, dr.Offer!.GetName(), CellValues.String, 0);
        //        InsertExcelCell(row, 2, dr.Offer.Price.ToString(), CellValues.String, 0);
        //        InsertExcelCell(row, 3, dr.Quantity.ToString(), CellValues.String, 0);
        //        InsertExcelCell(row, 4, dr.WeightOffer.ToString(), CellValues.String, 0);
        //        InsertExcelCell(row, 5, dr.Amount.ToString(), CellValues.String, 0);
        //        sheetData.Append(row);
        //    }
        //    Row btRow = new() { RowIndex = row_index++ };
        //    InsertExcelCell(btRow, 1, "", CellValues.String, 0);
        //    InsertExcelCell(btRow, 2, "", CellValues.String, 0);
        //    InsertExcelCell(btRow, 3, "Итого:", CellValues.String, 0);
        //    InsertExcelCell(btRow, 4, table.Rows!.Sum(x => x.WeightOffer).ToString(), CellValues.String, 0);
        //    InsertExcelCell(btRow, 5, table.Rows!.Sum(x => x.Amount).ToString(), CellValues.String, 0);
        //    sheetData.Append(btRow);
        //    sheetId++;
        //}

        //workbookPart.Workbook.Save();
        //spreadsheetDoc.Save();

        //XLSStream.Position = 0;
        return HTMLStream.ToArray();
    }*/
    #endregion
}