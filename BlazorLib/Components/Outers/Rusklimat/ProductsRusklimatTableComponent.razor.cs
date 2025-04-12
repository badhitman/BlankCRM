////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Outers.Rusklimat;

/// <summary>
/// ProductsRusklimatTableComponent
/// </summary>
public partial class ProductsRusklimatTableComponent : BlazorBusyComponentBaseModel
{
     [Inject]
    IRusklimatComApiTransmission rusklimatTrans { get; set; } = default!;


    private MudTable<ProductRusklimatModelDB>? table;

    private string? searchString = null;
    private async Task<TableData<ProductRusklimatModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        RusklimatRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
            SimpleRequest = searchString
        };

        await SetBusyAsync(token: token);
        TPaginationResponseModel<ProductRusklimatModelDB> res = await rusklimatTrans.ProductsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<ProductRusklimatModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async void OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}