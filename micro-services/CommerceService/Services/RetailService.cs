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
    #region Deliveries service`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryServiceAsync(DeliveryServiceRetailModelDB req, CancellationToken token = default)
    {
        TResponseModel<int> res = new();
        if (string.IsNullOrWhiteSpace(req.Name))
        {
            res.AddError("Укажите имя");
            return res;
        }

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.CreatedAtUTC = DateTime.UtcNow;

        if (await context.DeliveryRetailServices.AnyAsync(x => x.Name == req.Name, cancellationToken: token))
        {
            res.AddError("Служба доставки с таким именем уже существует");
            return res;
        }

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
    public async Task<TPaginationResponseModel<DeliveryServiceRetailModelDB>> SelectDeliveryServicesAsync(TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryServiceRetailModelDB>? q = context.DeliveryRetailServices.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

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
    public async Task<TResponseModel<DeliveryServiceRetailModelDB[]>> DeliveryServicesGetAsync(int[] reqIds, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        return new()
        {
            Response = await context.DeliveryRetailServices.Where(x => reqIds.Contains(x.Id)).ToArrayAsync(cancellationToken: token)
        };
    }
    #endregion

    #region Delivery Document
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

        req.CreatedAtUTC = DateTime.UtcNow;
        req.Orders = null;
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.DeliveryCode = req.DeliveryCode?.Trim();

        await context.DeliveryRetailDocuments.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        res.Response = req.Id;
        return res;
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryDocumentAsync(DeliveryDocumentRetailModelDB req, CancellationToken token = default)
    {
        req.CreatedAtUTC = DateTime.UtcNow;
        req.Orders = null;
        req.Name = req.Name.Trim();
        req.Description = req.Description?.Trim();
        req.DeliveryCode = req.DeliveryCode?.Trim();

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveryRetailDocuments
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
                .SetProperty(p => p.DeliveryStatus, context.DeliveryStatusesRetailDocuments.Where(y => y.DeliveryDocumentId == req.Id).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault())
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>> SelectDeliveryDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryDocumentRetailModelDB>? q = context.DeliveryRetailDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

        if (req.Payload?.RecipientsFilterIdentityId is not null && req.Payload.RecipientsFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.RecipientsFilterIdentityId.Contains(x.RecipientIdentityUserId));

        if (req.Payload?.TypesFilter is not null && req.Payload.TypesFilter.Length != 0)
            q = q.Where(x => req.Payload.TypesFilter.Contains(x.DeliveryType));

        if (req.Payload?.FilterOrderId is not null && req.Payload.FilterOrderId > 0)
            q = from deliveryDoc in q
                join linkItem in context.DeliveriesOrdersLinks.Where(x => x.OrderDocumentId == req.Payload.FilterOrderId) on deliveryDoc.Id equals linkItem.DeliveryDocumentId
                select deliveryDoc;

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
            Response = await pq
                .Include(x => x.DeliveryStatusesLog)
                .ToListAsync(cancellationToken: token)
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
            Response = await context.DeliveryRetailDocuments
                .Where(x => req.Ids.Contains(x.Id))
                .ToArrayAsync(cancellationToken: token)
        };
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

        await context.RowsDeliveryRetailDocuments.AddAsync(req, token);
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

        await context.RowsDeliveryRetailDocuments
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
        IQueryable<RowOfDeliveryRetailDocumentModelDB>? q = context.RowsDeliveryRetailDocuments.Where(x => x.DocumentId == req.Payload.DeliveryDocumentId).AsQueryable();

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
    #endregion

    #region Status Delivery Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        req.DateOperation = req.DateOperation.SetKindUtc();
        req.DeliveryDocument = null;
        req.Name = req.Name.Trim();

        await context.DeliveryStatusesRetailDocuments.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.DeliveryRetailDocuments
            .Where(x => x.Id == req.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveryStatusesRetailDocuments.Where(y => y.DeliveryDocumentId == req.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateDeliveryStatusDocumentAsync(DeliveryStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        await context.DeliveryStatusesRetailDocuments
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.DeliveryStatus, req.DeliveryStatus)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await context.DeliveryRetailDocuments
            .Where(x => x.Id == req.DeliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.DeliveryStatus, context.DeliveryStatusesRetailDocuments.Where(y => y.DeliveryDocumentId == req.DeliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.DeliveryStatus).FirstOrDefault()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>> SelectDeliveryStatusesDocumentsAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB>? q = context.DeliveryStatusesRetailDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

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
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<WalletRetailTypeModelDB>? q = context.WalletsRetailTypes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

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
            SortIndex = x.SortIndex,
            IsSystem = x.IsSystem,
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
                IsSystem = x.IsSystem,
                SortIndex = x.SortIndex,
                LastUpdatedAtUTC = x.LastUpdatedAtUTC,
                Name = x.Name,
                SumBalances = context.WalletsRetail.Where(y => y.WalletTypeId == x.Id).Sum(y => y.Balance)
            }).ToArrayAsync(cancellationToken: token)
        };
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
        IQueryable<WalletRetailModelDB>? q = context.WalletsRetail.AsQueryable();

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

        List<WalletRetailModelDB> res = await pq.Include(x => x.WalletType).ToListAsync(cancellationToken: token);

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
                    if (!res.Any(x => x.WalletTypeId == walletType.Id && x.UserIdentityId == userId) && (!walletType.IsSystem || getUsers.Response.First(u => u.UserId == userId).IsAdmin))
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
    #endregion

    #region Payment Document
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreatePaymentDocumentAsync(PaymentRetailDocumentModelDB req, CancellationToken token = default)
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
        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

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

        await context.PaymentsRetailDocuments.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        if (req.StatusPayment == PaymentsRetailStatusesEnum.Paid)
        {
            await context.WalletsRetail
             .Where(x => x.Id == req.WalletId)
             .ExecuteUpdateAsync(set => set
                 .SetProperty(p => p.Balance, p => p.Balance + req.Amount), cancellationToken: token);
        }

        await transaction.CommitAsync(token);

        res.Response = req.Id;
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
        IQueryable<PaymentRetailDocumentModelDB>? q = context.PaymentsRetailDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

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

            q = q.Where(x => context.WalletsRetail.Any(y => y.Id == x.WalletId && y.UserIdentityId == req.Payload.PayerFilterIdentityId));
        }

        if (req.Payload?.TypesFilter is not null && req.Payload.TypesFilter.Length != 0)
            q = q.Where(x => req.Payload.TypesFilter.Contains(x.TypePayment));

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Length != 0)
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.StatusPayment));

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

    #region Rows for Order-Document
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

        await context.RowsRetailsOrders.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.RetailOrders
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

        await context.RowsRetailsOrders
          .Where(x => x.Id == req.Id)
          .ExecuteUpdateAsync(set => set
              .SetProperty(p => p.Comment, req.Comment)
              .SetProperty(p => p.Quantity, req.Quantity)
              .SetProperty(p => p.Version, Guid.NewGuid())
              .SetProperty(p => p.Amount, req.Amount), cancellationToken: token);

        await context.RetailOrders
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
        IQueryable<RowOfRetailOrderDocumentModelDB>? q = context.RowsRetailsOrders.Where(x => x.OrderId == req.Payload.OrderId).AsQueryable();

        IQueryable<RowOfRetailOrderDocumentModelDB>? pq = q
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = await pq.Include(x => x.Offer).ToListAsync(cancellationToken: token)
        };
    }
    #endregion

    #region Order`s (document`s)
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

        if (req.Rows is not null && req.Rows.Count != 0)
        {
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
                .SetProperty(p => p.Name, req.Name.Trim())
                .SetProperty(p => p.DateDocument, req.DateDocument.SetKindUtc())
                .SetProperty(p => p.StatusDocument, req.StatusDocument)
                .SetProperty(p => p.BuyerIdentityUserId, req.BuyerIdentityUserId)
                .SetProperty(p => p.Version, Guid.NewGuid())
                .SetProperty(p => p.Description, req.Description)
                .SetProperty(p => p.WarehouseId, req.WarehouseId)
                .SetProperty(p => p.ExternalDocumentId, req.ExternalDocumentId)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailDocumentModelDB>> SelectRetailDocumentsAsync(TPaginationRequestStandardModel<SelectRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<RetailDocumentModelDB> q = context.RetailOrders.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery) || (x.Description != null && x.Description.Contains(req.FindQuery)));

        if (req.Payload?.BuyersFilterIdentityId is not null && req.Payload.BuyersFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.BuyersFilterIdentityId.Contains(x.BuyerIdentityUserId));

        if (req.Payload?.CreatorsFilterIdentityId is not null && req.Payload.CreatorsFilterIdentityId.Length != 0)
            q = q.Where(x => req.Payload.CreatorsFilterIdentityId.Contains(x.AuthorIdentityUserId));

        if (req.Payload?.WithoutDeliveryOnly == true)
            q = q.Where(x => !context.DeliveriesOrdersLinks.Any(y => y.OrderDocumentId == x.Id));

        IQueryable<RetailDocumentModelDB> pq = q
            .OrderBy(x => x.CreatedAtUTC)
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
                .Include(x => x.Rows)
                .Include(x => x.Deliveries)
                .ToListAsync(cancellationToken: token)
        };
    }

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
                         .Include(x => x.Deliveries)
                         .ToArrayAsync(cancellationToken: token)
        };

        if (res.Response.Length != req.Ids.Length)
            res.AddError("Некоторые документы не найдены");

        return res;
    }
    #endregion

    #region Conversion`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
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
        WalletRetailModelDB walletSenderDb = await context.WalletsRetail.Include(x => x.WalletType).FirstAsync(x => x.Id == req.FromWalletId, cancellationToken: token);

        if (!walletSenderDb.WalletType!.IsSystem && walletSenderDb.Balance - req.FromWalletSum < 0)
            return new() { Messages = [new() { TypeMessage = MessagesTypesEnum.Error, Text = "Баланс не может стать отрицательным в следствии списания" }] };

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Balance, p => p.Balance - req.FromWalletSum), cancellationToken: token);

        await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Balance, p => p.Balance + req.ToWalletSum), cancellationToken: token);

        req.Name = req.Name.Trim();
        req.Version = Guid.NewGuid();
        req.ToWallet = null;
        req.FromWallet = null;
        req.CreatedAtUTC = DateTime.UtcNow;
        req.DateDocument = req.DateDocument.SetKindUtc();

        await context.ConversionsDocumentsWalletsRetail.AddAsync(req, token);
        await context.SaveChangesAsync(token);
        await transaction.CommitAsync(token);
        return new TResponseModel<int>() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateConversionDocumentAsync(WalletConversionRetailDocumentModelDB req, CancellationToken token = default)
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
        if (_conversionDocDb.Version != req.Version)
            return ResponseBaseModel.CreateError("Документ уже кем-то изменён. Обновите страницу с документом и повторите попытку");

        decimal
            _deltaSender = req.FromWalletSum - _conversionDocDb.FromWalletSum,
            _deltaRecipient = req.ToWalletSum - _conversionDocDb.ToWalletSum;

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        if (req.FromWalletId == _conversionDocDb.FromWalletId)
        {
            await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - _deltaSender), cancellationToken: token);
        }
        else
        {
            await context.WalletsRetail.Where(x => x.Id == _conversionDocDb.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance + _conversionDocDb.FromWalletSum), cancellationToken: token);

            await context.WalletsRetail.Where(x => x.Id == req.FromWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - req.FromWalletSum), cancellationToken: token);
        }

        if (req.ToWalletId == _conversionDocDb.ToWalletId)
        {
            await context.WalletsRetail.Where(x => x.Id == req.ToWalletId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Balance, p => p.Balance + _deltaRecipient), cancellationToken: token);
        }
        else
        {
            await context.WalletsRetail.Where(x => x.Id == _conversionDocDb.ToWalletId)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(p => p.Balance, p => p.Balance - _conversionDocDb.ToWalletSum), cancellationToken: token);

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
    public async Task<TResponseModel<WalletConversionRetailDocumentModelDB[]>> GetConversionsDocumentsAsync(ReadWalletsRetailsConversionDocumentsRequestModel req, CancellationToken token = default)
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
    public async Task<TPaginationResponseModel<WalletConversionRetailDocumentModelDB>> SelectConversionsDocumentsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsConversionDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WalletConversionRetailDocumentModelDB> q = context.ConversionsDocumentsWalletsRetail.AsQueryable();

        if (!string.IsNullOrWhiteSpace(req.FindQuery))
            q = q.Where(x => x.Name.Contains(req.FindQuery));

        string[]
            sendersUserFilter = req.Payload?.SendersUserFilter ?? [],
            recipientsUserFilter = req.Payload?.RecipientsUserFilter ?? [];

        q = from doc in q
            join sender in context.WalletsRetail on doc.FromWalletId equals sender.Id
            join recipient in context.WalletsRetail on doc.ToWalletId equals recipient.Id

            where sendersUserFilter.Length == 0 || sendersUserFilter.Contains(sender.UserIdentityId)
            where recipientsUserFilter.Length == 0 || recipientsUserFilter.Contains(recipient.UserIdentityId)

            select doc;

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
            Response = await pq
                .Include(x => x.FromWallet).ThenInclude(x => x!.WalletType)
                .Include(x => x.ToWallet).ThenInclude(x => x!.WalletType)
                .ToListAsync(cancellationToken: token)
        };
    }
    #endregion
}