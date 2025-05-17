////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// InstrumentsTableStockSharpComponent
/// </summary>
public partial class InstrumentsTableStockSharpComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDataStockSharpService SsRepo { get; set; } = default!;

    MudTable<InstrumentTradeStockSharpViewModel>? _tableRef;
    private string? searchString = null;

    async Task OnSearch(string text)
    {
        searchString = text;
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    async Task FavoriteToggle(InstrumentTradeStockSharpViewModel sender)
    {
        await SetBusyAsync();
        ResponseBaseModel res = await SsRepo.InstrumentFavoriteToggleAsync(sender);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        if (_tableRef is not null)
            await _tableRef.ReloadServerData();
    }

    async Task<TableData<InstrumentTradeStockSharpViewModel>> ServerReload(TableState state, CancellationToken token)
    {
        InstrumentsRequestModel req = new()
        {
            FindQuery = searchString,
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> res = await SsRepo.InstrumentsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<InstrumentTradeStockSharpViewModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}