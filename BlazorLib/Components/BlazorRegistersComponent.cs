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
    public List<OfferAvailabilityModelDB> RegistersCache { get; private set; } = [];

    int[] offers = [];
    int[] goods = [];
    int warehouseId = 0;


    /// <inheritdoc/>
    public void SetRegistersCache(List<OfferAvailabilityModelDB> registersCache)
    {
        RegistersCache = registersCache;
        StateHasChanged();
    }

    /// <summary>
    /// CacheRegistersUpdate
    /// </summary>
    public async Task CacheRegistersUpdate(int[] _offers, int[] _goods, int _warehouseId = 0, bool clearCache = false)
    {
        if (clearCache)
        {
            lock (this)
            {
                RegistersCache.Clear();
            }
        }

        offers = [.. _offers.Where(x => x > 0 && !RegistersCache.Any(y => y.Id == x)).Distinct()];
        goods = [.. _goods.Where(x => x > 0 && !RegistersCache.Any(y => y.Id == x)).Distinct()];

        if (goods.Length == 0 && offers.Length == 0 && warehouseId < 1)
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
            },
            PageNum = 0,
            PageSize = 100,
            SortingDirection = DirectionsEnum.Up,
        };
        await SetBusyAsync();
        TPaginationResponseStandardModel<OfferAvailabilityModelDB> offersRegisters = await CommerceRepo.OffersRegistersSelectAsync(reqData);

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