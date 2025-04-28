////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Outers.Haier;

/// <summary>
/// ProductsHaierTableComponent
/// </summary>
public partial class ProductsHaierTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IFeedsHaierProffRuService haierTrans { get; set; } = default!;


    private MudTable<ProductHaierModelDB>? table;

    private string? searchString = null;
    private async Task<TableData<ProductHaierModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        HaierRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
            FindQuery = searchString
        };

        await SetBusyAsync(token: token);
        TPaginationResponseModel<ProductHaierModelDB> res = await haierTrans.ProductsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<ProductHaierModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async void OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}