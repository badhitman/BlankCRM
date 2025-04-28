////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// InstrumentsTableComponent
/// </summary>
public partial class InstrumentsTableComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStockSharpDriverService SsRepo { get; set; } = default!;

    private async Task<TableData<InstrumentTradeStockSharpModel>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<InstrumentsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            Payload = new(),
            SortingDirection = state.SortDirection == SortDirection.Ascending ? DirectionsEnum.Up : DirectionsEnum.Down,
        };
        await SetBusyAsync(token: token);
        TPaginationResponseModel<InstrumentTradeStockSharpModel> res = await SsRepo.InstrumentsSelectAsync(req, token);
        await SetBusyAsync(false, token: token);
        return new TableData<InstrumentTradeStockSharpModel>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }
}