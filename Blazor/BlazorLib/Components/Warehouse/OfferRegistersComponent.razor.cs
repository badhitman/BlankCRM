////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Warehouse;

/// <summary>
/// OfferRegistersComponent
/// </summary>
public partial class OfferRegistersComponent : BlazorBusyComponentRubricsCachedModel
{
    /// <summary>
    /// Commerce
    /// </summary>
    [Inject]
    protected ICommerceTransmission CommerceRepo { get; set; } = default!;


    MudTable<OfferAvailabilityModelDB>? table;

    List<RubricNestedModel> Warehouses = [];

    IReadOnlyCollection<int> _selectedWarehouses = [];
    IReadOnlyCollection<int> SelectedWarehouses
    {
        get => _selectedWarehouses;
        set
        {
            _selectedWarehouses = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        RubricsListRequestStandardModel req = new()
        {
            ContextName = Routes.WAREHOUSE_CONTROLLER_NAME,
        };

        Warehouses = await RubricsRepo.RubricsChildListAsync(req);
        await SetBusyAsync(false);
    }

    async Task ReloadTable()
    {
        if (table is null)
            return;

        await SetBusyAsync();
        await table.ReloadServerData();
        await SetBusyAsync(false);
    }

    /// <summary>
    /// Here we simulate getting the paged, filtered and ordered data from the server
    /// </summary>
    async Task<TableData<OfferAvailabilityModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        await SetBusyAsync(token: token);
        TPaginationRequestStandardModel<RegistersSelectRequestBaseModel> req = new()
        {
            Payload = new(),
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };

        if (SelectedWarehouses.Count != 0 && SelectedWarehouses.Count != Warehouses.Count)
        {
            req.Payload.WarehousesFilter = [.. SelectedWarehouses];
        }

        TPaginationResponseStandardModel<OfferAvailabilityModelDB> rest = await CommerceRepo.OffersRegistersSelectAsync(req, token);

        if (rest.Response is not null)
        {
            await CacheRubricsUpdate(rest.Response.Select(x => x.WarehouseId));
            await SetBusyAsync(false, token: token);
            return new TableData<OfferAvailabilityModelDB>() { TotalItems = rest.TotalRowsCount, Items = rest.Response };
        }

        await SetBusyAsync(false, token: token);
        return new TableData<OfferAvailabilityModelDB>() { TotalItems = 0, Items = [] };
    }
}