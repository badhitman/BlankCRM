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
    public async Task<TPaginationResponseStandardModel<WalletRetailTypeViewModel>> SelectWalletsTypesAsync(TPaginationRequestStandardModel<SelectWalletsRetailsTypesRequestModel> req, CancellationToken token = default)
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
}