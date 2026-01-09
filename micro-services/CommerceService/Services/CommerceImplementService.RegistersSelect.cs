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
    public async Task<TPaginationResponseStandardModel<OfferAvailabilityModelDB>> OffersRegistersSelectAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req, CancellationToken token = default)
    {
        if (req.Payload is null)
        {
            loggerRepo.LogError("req.Payload is null");
            return new();
        }

        if (req.PageSize < 10)
            req.PageSize = 10;

        using CommerceContext context = await commerceDbFactory.CreateDbContextAsync(token);

        IQueryable<OfferAvailabilityModelDB> q = context
            .OffersAvailability
            .AsQueryable();

        if (req.Payload.MinQuantity.HasValue)
            q = q.Where(x => x.Quantity >= req.Payload.MinQuantity);

        if (req.Payload.OfferFilter is not null && req.Payload.OfferFilter.Length != 0)
            q = q.Where(x => req.Payload.OfferFilter.Any(y => y == x.OfferId));

        if (req.Payload.NomenclatureFilter is not null && req.Payload.NomenclatureFilter.Length != 0)
            q = q.Where(x => req.Payload.NomenclatureFilter.Any(y => y == x.NomenclatureId));

        if (req.Payload.WarehousesFilter is not null && req.Payload.WarehousesFilter.Length != 0)
            q = q.Where(x => req.Payload.WarehousesFilter.Contains(x.WarehouseId));

        var exQuery = from offerAv in q
                      join oj in context.Offers on offerAv.OfferId equals oj.Id
                      join gj in context.Nomenclatures on offerAv.NomenclatureId equals gj.Id
                      select new { Register = offerAv, Offer = oj, Good = gj };

        var dbRes = req.SortingDirection == DirectionsEnum.Up
           ? await exQuery.OrderBy(x => x.Offer.Name).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToArrayAsync(cancellationToken: token)
           : await exQuery.OrderByDescending(x => x.Offer.Name).Skip(req.PageNum * req.PageSize).Take(req.PageSize).ToArrayAsync(cancellationToken: token);

        return new()
        {
            PageNum = req.PageNum,
            PageSize = req.PageSize,
            SortingDirection = req.SortingDirection,
            SortBy = req.SortBy,
            TotalRowsCount = await q.CountAsync(cancellationToken: token),
            Response = [.. dbRes.Select(x => x.Register)],
        };
    }
}