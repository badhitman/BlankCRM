////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Outers.Daichi;

/// <summary>
/// ProductsDaichiTableComponent
/// </summary>
public partial class ProductsDaichiTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDaichiBusinessApiTransmission daichiTrans { get; set; } = default!;


    private MudTable<ProductDaichiModelDB>? table;

    private string? searchString = null;
    private async Task<TableData<ProductDaichiModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        DaichiRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
            FindQuery = searchString
        };

        await SetBusyAsync(token: token);
        TPaginationResponseModel<ProductDaichiModelDB> res = await daichiTrans.ProductsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<ProductDaichiModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async void OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}