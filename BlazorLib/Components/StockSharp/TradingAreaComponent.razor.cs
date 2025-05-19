////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// TradingAreaComponent
/// </summary>
public partial class TradingAreaComponent : StockSharpBaseComponent
{
    /// <inheritdoc/>
    [Inject]
    protected IDataStockSharpService DataRepo { get; set; } = default!;


    List<InstrumentTradeStockSharpViewModel>? instruments;

    async Task StartTradeAsync()
    {
        await SetBusyAsync();
        await SetBusyAsync(false);
    }
    async Task StopTradeAsync()
    {
        await SetBusyAsync();
        await SetBusyAsync(false);
    }
    async Task DownloadBaseAsync()
    {
        await SetBusyAsync();
        await SetBusyAsync(false);
    }
    async Task ResetAllTradesAsync()
    {
        await SetBusyAsync();
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        InstrumentsRequestModel req = new()
        {
            PageNum = 0,
            PageSize = int.MaxValue,
            FavoriteFilter = true,
        };
        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> res = await DataRepo.InstrumentsSelectAsync(req);
        instruments = res.Response;
        await SetBusyAsync(false);
    }
}