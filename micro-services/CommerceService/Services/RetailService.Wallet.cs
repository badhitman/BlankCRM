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
public partial class RetailService : IRetailService
{
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
    public async Task<TPaginationResponseStandardModel<WalletRetailModelDB>> SelectWalletsAsync(TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> req, CancellationToken token = default)
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
}