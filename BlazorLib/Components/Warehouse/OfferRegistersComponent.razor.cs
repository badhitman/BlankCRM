////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using MudBlazor;
using SharedLib;
using static SharedLib.GlobalStaticConstantsRoutes;

namespace BlazorLib.Components.Warehouse;

/// <summary>
/// OfferRegistersComponent
/// </summary>
public partial class OfferRegistersComponent : BlazorBusyComponentRubricsCachedModel
{
    MudTable<OfferAvailabilityModelDB>? table;

    List<UniversalBaseModel> Warehauses = [];

    IReadOnlyCollection<int> _selectedWarehauses = [];
    IReadOnlyCollection<int> SelectedWarehauses
    {
        get => _selectedWarehauses;
        set
        {
            _selectedWarehauses = value;
            if (table is not null)
                InvokeAsync(table.ReloadServerData);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        RubricsListRequestModel req = new()
        {
            ContextName = Routes.WAREHOUSE_CONTROLLER_NAME,
        };

        Warehauses = await HelpDeskRepo.RubricsChildListAsync(req);
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
            Payload = new()
            {
                MinQuantity = 1,
            },
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortBy = state.SortLabel,
            SortingDirection = state.SortDirection.Convert(),
        };

        if (SelectedWarehauses.Count != 0 && SelectedWarehauses.Count != Warehauses.Count)
        {
            req.Payload.WarehousesFilter = [.. SelectedWarehauses];
        }

        TPaginationResponseModel<OfferAvailabilityModelDB> rest = await CommerceRepo.OffersRegistersSelectAsync(req, token);

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