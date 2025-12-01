////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Розница
/// </summary>
public class RetailService(IIdentityTransmission identityRepo,
    IKladrNavigationService kladrRepo,
    IDbContextFactory<CommerceContext> commerceDbFactory) : IRetailService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<RetailDocumentModelDB[]>> RetailDocumentsGetAsync(RetailDocumentsGetRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RetailDocumentModelDB>? q = context.RetailOrders
            .Where(x => req.Ids.Contains(x.Id));

        TResponseModel<RetailDocumentModelDB[]> res = new()
        {
            Response = !req.IncludeDataExternal
                ? await q.ToArrayAsync(cancellationToken: token)
                : await q.Include(x => x.Rows)
                         .Include(x => x.DeliveryDocuments)
                         .Include(x => x.PaymentsLinks)
                         .ToArrayAsync(cancellationToken: token)
        };

        if (res.Response.Length != req.Ids.Length)
            res.AddError("Некоторые документы не найдены");

        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRetailDocumentAsync(RetailDocumentModelDB req, CancellationToken token = default)
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

        TResponseModel<UserInfoModel[]> getUsers = await identityRepo.GetUsersOfIdentityAsync([req.AuthorIdentityUserId, req.BuyerIdentityUserId], token);
        if (!getUsers.Success())
        {
            res.AddRangeMessages(getUsers.Messages);
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.Version = Guid.NewGuid();
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        await context.RetailOrders.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new TResponseModel<int>() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRetailDocumentAsync(RetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.RetailOrders
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.StatusDocument, req.StatusDocument)
                .SetProperty(p => p.BuyerIdentityUserId, req.BuyerIdentityUserId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.WarehouseId, req.WarehouseId)
                //.SetProperty(p => p.HelpDeskId, req.HelpDeskId)
                .SetProperty(p => p.ExternalDocumentId, req.ExternalDocumentId)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailDocumentModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RetailDocumentModelDB> q = context.RetailOrders.AsQueryable();

        if (req.Payload?.BuyersFilterIdentityId is not null && req.Payload.BuyersFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.BuyersFilterIdentityId.Contains(x.BuyerIdentityUserId));

        if (req.Payload?.CreatorsFilterIdentityId is not null && req.Payload.CreatorsFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.CreatorsFilterIdentityId.Contains(x.AuthorIdentityUserId));

        IQueryable<RetailDocumentModelDB> pq = q
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
                .OrderBy(x => x.CreatedAtUTC)
                .Include(x => x.Rows)
                .Include(x => x.PaymentsLinks)
                .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.ConversionsDocumentsWalletsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new TResponseModel<int>() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
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

        await context.DeliveryRetailDocuments.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        res.Response = req.Id;
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (string.IsNullOrWhiteSpace(req.Name))
        {
            res.AddError("Укажите имя");
            return res;
        }

        req.Name = req.Name.Trim();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        if (await context.DeliveryRetailServices.AnyAsync(x => x.Name == req.Name, cancellationToken: token))
        {
            res.AddError("Служба доставки с таким именем уже существует");
            return res;
        }

        req.Description = req.Description?.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        if (await context.DeliveryRetailServices.AnyAsync(cancellationToken: token))
        {
            req.SortIndex = await context.DeliveryRetailServices.MaxAsync(x => x.SortIndex, cancellationToken: token);
            req.SortIndex++;
        }
        else
            req.SortIndex = 1;

        await context.DeliveryRetailServices.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.DeliveryStatusesRetailDocuments.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
    {
        req.Version = Guid.NewGuid();
        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        TResponseModel<UserInfoModel[]> user = await identityRepo.GetUsersOfIdentityAsync([req.PayerIdentityUserId], token);
        if (!user.Success())
        {
            res.AddRangeMessages(user.Messages);
            return res;
        }

        await context.PaymentsRetailDocuments.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        res.Response = req.Id;
        return res;
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.PaymentsOrdersRetailLinks.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        req.Version = Guid.NewGuid();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        await context.RowsDeliveryRetailDocuments.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateWalletAsync(WalletRetailModelDB req, CancellationToken token = default)
    {
        req.Version = Guid.NewGuid();
        TResponseModel<int> res = new();
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        TResponseModel<UserInfoModel[]> user = await identityRepo.GetUsersOfIdentityAsync([req.UserIdentityId], token);
        if (!user.Success())
        {
            res.AddRangeMessages(user.Messages);
            return res;
        }

        await context.WalletsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        res.Response = req.Id;
        return res;
    }

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

        if (await context.WalletsRetailTypes.AnyAsync(cancellationToken: token))
        {
            req.SortIndex = await context.WalletsRetailTypes.MaxAsync(x => x.SortIndex, cancellationToken: token);
            req.SortIndex++;
        }
        else
            req.SortIndex = 1;

        await context.WalletsRetailTypes.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletConversionRetailDocumentModelDB> q = context.ConversionsDocumentsWalletsRetail.AsQueryable();

        IQueryable<WalletConversionRetailDocumentModelDB> pq = q
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
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryDocumentRetailModelDB>? q = context.DeliveryRetailDocuments.AsQueryable();

        if (req.Payload?.RecipientsFilterIdentityId is not null && req.Payload.RecipientsFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.RecipientsFilterIdentityId.Contains(x.RecipientIdentityUserId));

        if (req.Payload?.FilterOrderId is not null && req.Payload.FilterOrderId > 0)
            q = q.Where(x => req.Payload.FilterOrderId == x.OrderId);

        IQueryable<DeliveryDocumentRetailModelDB>? pq = q
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
    public async Task<TPaginationResponseModel<DeliveryServiceRetailModelDB>> SelectDeliveryServicesAsync(TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryServiceRetailModelDB>? q = context.DeliveryRetailServices.AsQueryable();

        IQueryable<DeliveryServiceRetailModelDB>? pq = q
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq.OrderBy(x => x.SortIndex).ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB>? q = context.DeliveryStatusesRetailDocuments.AsQueryable();

        IQueryable<DeliveryStatusRetailDocumentModelDB>? pq = q
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
    public async Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>> SelectPaymentsDocumentsAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<PaymentRetailDocumentModelDB>? q = context.PaymentsRetailDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.Payload?.PayerFilterIdentityId))
        {
            TResponseModel<UserInfoModel[]> user = await identityRepo.GetUsersOfIdentityAsync([req.Payload.PayerFilterIdentityId], token);
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

            q = q.Where(x => x.PayerIdentityUserId == req.Payload.PayerFilterIdentityId);
        }

        IQueryable<PaymentRetailDocumentModelDB>? pq = q
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
                .Include(x => x.Wallet)
                .Include(x => x.PaymentOrdersLinks)!
                .ThenInclude(x => x.Order)
                .ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailOrderLinkModelDB>> SelectPaymentsOrdersLinksAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersLinksRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<PaymentRetailOrderLinkModelDB>? q = context.PaymentsOrdersRetailLinks.AsQueryable();

        IQueryable<PaymentRetailOrderLinkModelDB>? pq = q
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
    public async Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>> SelectRowOfDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RowOfDeliveryRetailDocumentModelDB>? q = context.RowsDeliveryRetailDocuments.AsQueryable();

        IQueryable<RowOfDeliveryRetailDocumentModelDB>? pq = q
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
    public async Task<TPaginationResponseModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletRetailModelDB>? q = context.WalletsRetail.AsQueryable();

        if (req.Payload?.UsersFilterIdentityId is not null && req.Payload.UsersFilterIdentityId.Length != 0)
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
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<WalletRetailModelDB> res = await pq.Include(x => x.WalletType).ToListAsync(cancellationToken: token);

        if (req.Payload?.AutoGenerationWallets == true)
        {
            WalletRetailTypeModelDB[] walletsTypesDb = await context.WalletsRetailTypes
                .Where(x => !x.IsDisabled)
                .ToArrayAsync(cancellationToken: token);

            List<WalletRetailModelDB> createWallets = [];

            res.GroupBy(x => x.UserIdentityId).ToList()
                .ForEach(gNode =>
                {
                    WalletRetailTypeModelDB[] subList = [.. walletsTypesDb.Where(w => !gNode.Any(y => y.WalletTypeId == w.Id))];
                    if (subList.Length != 0)
                    {
                        createWallets.AddRange(subList.Select(x => new WalletRetailModelDB()
                        {
                            UserIdentityId = gNode.Key,
                            CreatedAtUTC = DateTime.UtcNow,
                            Description = "",
                            Name = "auto generation",
                            Version = Guid.NewGuid(),
                            WalletTypeId = x.Id,
                        }));
                    }
                });

            if (createWallets.Count != 0)
            {
                await context.WalletsRetail.AddRangeAsync(createWallets, token);
                await context.SaveChangesAsync(cancellationToken: token);
                res = await pq.Include(x => x.WalletType).ToListAsync(cancellationToken: token);
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
    public async Task<TPaginationResponseModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletRetailTypeModelDB>? q = context.WalletsRetailTypes.AsQueryable();

        IQueryable<WalletRetailTypeModelDB>? pq = q
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<WalletRetailTypeViewModel> res = await pq.OrderBy(x => x.SortIndex).Select(x => new WalletRetailTypeViewModel()
        {
            Id = x.Id,
            CreatedAtUTC = x.CreatedAtUTC,
            Description = x.Description,
            LastUpdatedAtUTC = x.LastUpdatedAtUTC,
            Name = x.Name,
            IsDisabled = x.IsDisabled,
            SumBalances = context.WalletsRetail.Where(y => y.WalletTypeId == x.Id).Sum(y => y.Balance)
        }).ToListAsync(cancellationToken: token);

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
    public async Task<ResponseBaseModel> UpdateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.ConversionsDocumentsWalletsRetail
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.FromWalletId, req.FromWalletId)
                .SetProperty(p => p.FromWalletSum, req.FromWalletSum)
                .SetProperty(p => p.ToWalletId, req.ToWalletId)
                .SetProperty(p => p.ToWalletSum, req.ToWalletSum)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveryRetailDocuments
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.WeightShipping, req.WeightShipping)
                .SetProperty(p => p.ShippingCost, req.ShippingCost)
                .SetProperty(p => p.RecipientIdentityUserId, req.RecipientIdentityUserId)
                .SetProperty(p => p.Paid, req.Paid)
                .SetProperty(p => p.KladrTitle, req.KladrTitle)
                .SetProperty(p => p.KladrCode, req.KladrCode)
                .SetProperty(p => p.DeliveryType, req.DeliveryType)
                .SetProperty(p => p.DeliveryPayment, req.DeliveryPayment)
                .SetProperty(p => p.DeliveryCode, req.DeliveryCode)
                .SetProperty(p => p.AddressUserComment, req.AddressUserComment)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveryRetailServices
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveryStatusesRetailDocuments
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.DeliveryStatus, req.DeliveryStatus)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

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
                .SetProperty(p => p.PayerIdentityUserId, req.PayerIdentityUserId)
                .SetProperty(p => p.Amount, req.Amount)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdatePaymentOrderLinkAsync(PaymentRetailOrderLinkModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.PaymentsOrdersRetailLinks
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Amount, req.Amount)
                .SetProperty(p => p.Comment, req.Comment)
                .SetProperty(p => p.IsDisabled, req.IsDisabled), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateRowOfDeliveryDocumentAsync(RowOfDeliveryRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.RowsDeliveryRetailDocuments
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.OfferId, req.OfferId)
                .SetProperty(p => p.Quantity, req.Quantity)
                .SetProperty(p => p.Amount, req.Amount)
                .SetProperty(p => p.Comment, req.Comment)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.NomenclatureId, req.NomenclatureId)
                .SetProperty(p => p.IsDisabled, req.IsDisabled), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
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
    public async Task<ResponseBaseModel> UpdateWalletTypeAsync(WalletRetailTypeModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.WalletsRetailTypes
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.IsDisabled, req.IsDisabled)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/> 
    public async Task<ResponseBaseModel> WalletBalanceUpdateAsync(WalletBalanceCommitRequestModel req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.WalletsRetail
            .Where(x => x.Id == req.WalletId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Balance, p => p.Balance + req.ValueCommit)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/> 
    public async Task<TResponseModel<WalletRetailTypeViewModel[]>> WalletsTypesGetAsync(int[] reqIds, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        return new()
        {
            Response = await context.WalletsRetailTypes.Where(x => reqIds.Contains(x.Id)).Select(x => new WalletRetailTypeViewModel()
            {
                Id = x.Id,
                CreatedAtUTC = x.CreatedAtUTC,
                Description = x.Description,
                IsDisabled = x.IsDisabled,
                LastUpdatedAtUTC = x.LastUpdatedAtUTC,
                Name = x.Name,
                SumBalances = context.WalletsRetail.Where(y => y.WalletTypeId == x.Id).Sum(y => y.Balance)
            }).ToArrayAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/> 
    public async Task<TResponseModel<DeliveryServiceRetailModelDB[]>> DeliveryServicesGetAsync(int[] reqIds, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        return new()
        {
            Response = await context.DeliveryRetailServices.Where(x => reqIds.Contains(x.Id)).ToArrayAsync(cancellationToken: token)
        };
    }
}
