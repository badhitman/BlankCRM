////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using SharedLib;
using DbcLib;

namespace CommerceService;

/// <summary>
/// Commerce
/// </summary>
public partial class CommerceImplementService : ICommerceService
{
    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<WarehouseDocumentModelDB>> WarehouseDocumentsSelectAsync(TPaginationRequestStandardModel<WarehouseDocumentsSelectRequestModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError("req.Payload is null");
            return new();
        }

        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<WarehouseDocumentModelDB> q = context
            .WarehouseDocuments
            .AsQueryable();

        if (req.Payload.DisabledOnly.HasValue)
            q = q.Where(x => x.IsDisabled == req.Payload.DisabledOnly.Value);

        if (!string.IsNullOrWhiteSpace(req.Payload.SearchQuery))
            q = q.Where(x => x.NormalizedUpperName.Contains(req.Payload.SearchQuery.ToUpper()));

        if (req.Payload.OfferFilter is not null && req.Payload.OfferFilter.Length != 0)
            q = q.Where(x => context.RowsWarehouses.Any(y => y.WarehouseDocumentId == x.Id && req.Payload.OfferFilter.Any(i => i == y.OfferId)));

        if (req.Payload.NomenclatureFilter is not null && req.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => context.RowsWarehouses.Any(y => y.WarehouseDocumentId == x.Id && req.Payload.NomenclatureFilter.Any(i => i == y.NomenclatureId)));

        if (req.Payload.AfterDateUpdate is not null)
            q = q.Where(x => x.LastUpdatedAtUTC >= req.Payload.AfterDateUpdate || (x.LastUpdatedAtUTC == DateTime.MinValue && x.CreatedAtUTC >= req.Payload.AfterDateUpdate));

        if (req.Payload.AfterDeliveryDate is not null)
            q = q.Where(x => x.DeliveryDate >= req.Payload.AfterDeliveryDate || (x.DeliveryDate == DateTime.MinValue && x.DeliveryDate >= req.Payload.AfterDeliveryDate));


        IOrderedQueryable<WarehouseDocumentModelDB> oq = req.SortingDirection == DirectionsEnum.Up
           ? q.OrderBy(x => x.CreatedAtUTC)
           : q.OrderByDescending(x => x.CreatedAtUTC);

        IQueryable<WarehouseDocumentModelDB> pq = oq
            .Skip(req.PageNum * req.PageSize)
            .Take(req.PageSize);

        Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<WarehouseDocumentModelDB, NomenclatureModelDB?> inc_query = pq
            .Include(x => x.Rows!)
            .ThenInclude(x => x.Offer!)
            .ThenInclude(x => x.Nomenclature);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = req.Payload.IncludeExternalData ? [.. await inc_query.ToArrayAsync(cancellationToken: token)] : [.. await pq.ToArrayAsync(cancellationToken: token)]
        };
    }
}