////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Outers.Breez;

/// <summary>
/// ProductsBreezTableComponent
/// </summary>
public partial class ProductsBreezTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IBreezRuApiTransmission breezTrans { get; set; } = default!;


    private MudTable<ProductViewBreezRuModeld>? table;

    private string? searchString = null;
    private async Task<TableData<ProductViewBreezRuModeld>> ServerReload(TableState state, CancellationToken token)
    {
        BreezRequestModel req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            FindQuery = searchString
        };

        await SetBusyAsync(token: token);
        TPaginationResponseModel<ProductViewBreezRuModeld> res = await breezTrans.ProductsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<ProductViewBreezRuModeld>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    async void OnSearch(string text)
    {
        searchString = text;
        if (table is not null)
            await table.ReloadServerData();
    }
}