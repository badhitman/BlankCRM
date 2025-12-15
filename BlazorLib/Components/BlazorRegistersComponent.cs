////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib;

/// <summary>
/// BlazorRegistersComponent
/// </summary>
public abstract class BlazorRegistersComponent : BlazorBusyComponentBaseAuthModel
{
    /// <summary>
    /// Commerce
    /// </summary>
    [Inject]
    protected ICommerceTransmission CommerceRepo { get; set; } = default!;


    /// <summary>
    /// RegistersCache
    /// </summary>
    public List<OfferAvailabilityModelDB> RegistersCache = [];

    /// <summary>
    /// CacheRegistersUpdate
    /// </summary>
    protected async Task CacheRegistersUpdate(int[] offers, int[] goods, int warehouseId = 0, bool clearCache = false)
    {
        if (clearCache)
        {
            lock (this)
            {
                RegistersCache.Clear();
            }
        }

        offers = [.. offers.Where(x => x > 0 && !RegistersCache.Any(y => y.Id == x)).Distinct()];
        goods = [.. goods.Where(x => x > 0 && !RegistersCache.Any(y => y.Id == x)).Distinct()];

        if (goods.Length == 0 && offers.Length == 0)
        {
            StateHasChanged();
            return;
        }

        TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> reqData = new()
        {
            Payload = new()
            {
                OfferFilter = [.. offers],
                NomenclatureFilter = [.. goods],
                WarehousesFilter = warehouseId <= 0 ? null : [warehouseId],
                MinQuantity = 1,
            },
            PageNum = 0,
            PageSize = 100,
            SortingDirection = DirectionsEnum.Up,
        };
        await SetBusyAsync();
        TPaginationResponseModel<OfferAvailabilityModelDB> offersRegisters = await CommerceRepo.OffersRegistersSelectAsync(reqData);

        if (offersRegisters.TotalRowsCount > offersRegisters.PageSize)
            SnackBarRepo.Error($"Записей больше: {offersRegisters.TotalRowsCount}");

        if (offersRegisters.Response is not null && offersRegisters.Response.Count != 0)
        {
            lock (this)
            {
                RegistersCache.AddRange(offersRegisters.Response.Where(x => !RegistersCache.Any(y => y.Id == x.Id)));
            }
        }
        await SetBusyAsync(false);
    }
}