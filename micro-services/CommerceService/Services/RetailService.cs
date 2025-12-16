////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Text.RegularExpressions;
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
            await context.OrdersDeliveriesLinks.AddAsync(new() { DeliveryDocumentId = docDb.Id, OrderDocumentId = req.InjectToOrderId }, token);
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
        IQueryable<DeliveryDocumentRetailModelDB>? q = context.DeliveryDocumentsRetail.AsQueryable();

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

        IQueryable<DeliveryDocumentRetailModelDB>? pq = q
            .OrderBy(x => x.CreatedAtUTC)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<DeliveryDocumentRetailModelDB> res = await pq
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
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<DeliveryStatusRetailDocumentModelDB>? q = context.DeliveriesStatusesDocumentsRetail.AsQueryable();

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
                .SetProperty(p => p.IgnoreBalanceChanges, req.IgnoreBalanceChanges)
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
            .OrderBy(x => x.SortIndex)
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
            IgnoreBalanceChanges = x.IgnoreBalanceChanges,
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
            await context.PaymentsOrdersLinks.AddAsync(new() { OrderDocumentId = req.InjectToOrderId, PaymentDocumentId = docDb.Id }, token);
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

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DatePayment >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DatePayment <= req.Payload.End);
        }

        IQueryable<PaymentRetailDocumentModelDB>? pq = q
            .OrderBy(x => x.DatePayment)
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

        using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(token);

        DocumentRetailModelDB docDb = DocumentRetailModelDB.Build(req);

        await context.OrdersRetail.AddAsync(docDb, token);
        await context.SaveChangesAsync(token);
        res.AddSuccess($"Заказ успешно создан #{docDb.Id}");
        res.Response = docDb.Id;

        if (req.InjectToPaymentId > 0)
        {
            await context.PaymentsOrdersLinks.AddAsync(new() { OrderDocumentId = req.Id, PaymentDocumentId = req.InjectToPaymentId }, token);
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
            await context.OrdersDeliveriesLinks.AddAsync(new() { OrderDocumentId = req.Id, DeliveryDocumentId = req.InjectToDeliveryId }, token);
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

        if (req.Payload?.StatusesFilter is not null && req.Payload.StatusesFilter.Length != 0)
            q = q.Where(x => req.Payload.StatusesFilter.Contains(x.StatusDocument));

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DateDocument >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.Payload.End);
        }

        IQueryable<DocumentRetailModelDB> pq = q
            .OrderBy(x => x.DateDocument)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        List<DocumentRetailModelDB> res = await pq
                .Include(x => x.Rows)
                .Include(x => x.Deliveries)
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
        IQueryable<DocumentRetailModelDB>? q = context.OrdersRetail
            .Where(x => req.Ids.Contains(x.Id));

        TResponseModel<DocumentRetailModelDB[]> res = new()
        {
            Response = !req.IncludeDataExternal
                ? await q.ToArrayAsync(cancellationToken: token)
                : await q.Include(x => x.Rows!)
                         .ThenInclude(x => x.Offer)
                         .Include(x => x.Deliveries)
                         .ToArrayAsync(cancellationToken: token)
        };

        if (res.Response.Length != req.Ids.Length)
            res.AddError("Некоторые документы не найдены");

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
        IQueryable<RowOfRetailOrderDocumentModelDB>? q = context.RowsOrdersRetails.Where(x => x.OrderId == req.Payload.OrderId).AsQueryable();

        IQueryable<RowOfRetailOrderDocumentModelDB>? pq = q
            .OrderBy(x => x.Id)
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

        IQueryable<RetailOrderDeliveryLinkModelDB> pq = q
            .OrderBy(x => x.OrderDocumentId)
            .ThenBy(x => x.DeliveryDocumentId)
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = (forOrders && forDeliveries) || (!forOrders && !forDeliveries)
                            ? await pq.Include(x => x.DeliveryDocument).Include(x => x.OrderDocument).ToListAsync(cancellationToken: token)
                            : forOrders
                                ? await pq.Include(x => x.DeliveryDocument).ToListAsync(cancellationToken: token)
                                : await pq.Include(x => x.OrderDocument!).ToListAsync(cancellationToken: token)
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

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = (forOrders && forPayments) || (!forOrders && !forPayments)
                            ? await pq.Include(x => x.PaymentDocument).Include(x => x.OrderDocument).ToListAsync(cancellationToken: token)
                            : forOrders
                                ? await pq.Include(x => x.PaymentDocument).ToListAsync(cancellationToken: token)
                                : await pq.Include(x => x.OrderDocument!).ToListAsync(cancellationToken: token)
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

        await context.OrdersStatusesRetails.AddAsync(req, token);
        await context.SaveChangesAsync(token);

        await context.OrdersRetail
            .Where(x => x.Id == req.OrderDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        return new() { Response = req.Id };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UpdateOrderStatusDocumentAsync(OrderStatusRetailDocumentModelDB req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        req.Name = req.Name.Trim();
        await context.OrdersStatusesRetails
            .Where(x => x.Id == req.Id)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.Name, req.Name)
                .SetProperty(p => p.StatusDocument, req.StatusDocument)
                .SetProperty(p => p.LastUpdatedAtUTC, DateTime.UtcNow), cancellationToken: token);

        await context.OrdersRetail
            .Where(x => x.Id == req.OrderDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == req.OrderDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Ok");
    }

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderStatusRetailDocumentModelDB>> SelectOrderDocumentStatusesAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel> req, CancellationToken token = default)
    {
        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);
        IQueryable<OrderStatusRetailDocumentModelDB>? q = context.OrdersStatusesRetails.AsQueryable();

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

        await q.ExecuteDeleteAsync(cancellationToken: token);

        await context.OrdersRetail
            .Where(x => x.Id == deliveryDocumentId)
            .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.StatusDocument, context.OrdersStatusesRetails.Where(y => y.OrderDocumentId == deliveryDocumentId).OrderByDescending(z => z.DateOperation).Select(s => s.StatusDocument).FirstOrDefault()), cancellationToken: token);

        return ResponseBaseModel.CreateSuccess("Строка-статус успешно удалена");
    }
    #endregion

    #region Conversion`s
    /// <inheritdoc/>
    public async Task<TResponseModel<int>> CreateConversionDocumentAsync(CreateWalletConversionRetailDocumentRequestModel req, CancellationToken token = default)
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
            await context.ConversionsOrdersLinksRetail.AddAsync(new() { ConversionDocumentId = docDb.Id, OrderDocumentId = req.InjectToOrderId }, token);
            await context.SaveChangesAsync(token);
            res.AddInfo($"Добавлена связь документа перевода/конвертации #{docDb.Id} с заказом #{req.InjectToOrderId}");
        }

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

            where sendersUserFilter.Length == 0 || sendersUserFilter.Contains(sender.UserIdentityId)
            where recipientsUserFilter.Length == 0 || recipientsUserFilter.Contains(recipient.UserIdentityId)

            select doc;

        if (req.Payload?.Start is not null && req.Payload.Start != default)
            q = q.Where(x => x.DateDocument >= req.Payload.Start.SetKindUtc());

        if (req.Payload?.End is not null && req.Payload.End != default)
        {
            req.Payload.End = req.Payload.End.Value.AddHours(23).AddMinutes(59).AddSeconds(59).SetKindUtc();
            q = q.Where(x => x.DateDocument <= req.Payload.End);
        }

        IQueryable<WalletConversionRetailDocumentModelDB> pq = q
            .OrderBy(x => x.DateDocument)
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

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteToggleConversionAsync(int conversionId, CancellationToken token = default)
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
    public async Task<TResponseModel<int>> CreateConversionOrderLinkDocumentAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default)
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
    public async Task<ResponseBaseModel> UpdateConversionOrderLinkDocumentAsync(ConversionOrderRetailLinkModelDB req, CancellationToken token = default)
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
    public async Task<TPaginationResponseModel<ConversionOrderRetailLinkModelDB>> SelectConversionsOrdersDocumentsLinksAsync(TPaginationRequestStandardModel<SelectConversionsOrdersLinksRetailDocumentsRequestModel> req, CancellationToken token = default)
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

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = (forOrders && forConversions) || (!forOrders && !forConversions)
                            ? await pq.Include(x => x.ConversionDocument).Include(x => x.OrderDocument).ToListAsync(cancellationToken: token)
                            : forOrders
                                ? await pq.Include(x => x.ConversionDocument).ToListAsync(cancellationToken: token)
                                : await pq.Include(x => x.OrderDocument!).ToListAsync(cancellationToken: token)
        };
    }

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> DeleteConversionOrderLinkDocumentAsync(DeleteConversionOrderLinkRetailDocumentsRequestModel req, CancellationToken token = default)
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
}