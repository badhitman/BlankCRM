////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// InstrumentsTableComponent
/// </summary>
public partial class InstrumentsTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStockSharpDriverService SsRepo { get; set; } = default!;

    MudTable<InstrumentTradeStockSharpViewModel>? _tableRef;

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
        TPaginationRequestStandardModel<InstrumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new(),
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> res = await SsRepo.InstrumentsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<InstrumentTradeStockSharpViewModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}