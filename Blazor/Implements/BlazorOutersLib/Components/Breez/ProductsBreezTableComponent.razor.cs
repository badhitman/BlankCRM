using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;
using System.Text;
using BlazorLib;

namespace BlazorOutersLib.Components.Breez;

/// <summary>
/// ProductsBreezTableComponent
/// </summary>
public partial class ProductsBreezTableComponent
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
        TPaginationResponseStandardModel<ProductViewBreezRuModeld> res = await breezTrans.ProductsSelectAsync(req, token);
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
