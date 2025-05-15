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


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        TPaginationRequestStandardModel<InstrumentsRequestModel> req = new()
        {
            PageNum = 0,
            PageSize = int.MaxValue,
            Payload = new()
            {
                FavoriteFilter = true,
            }
        };
        TPaginationResponseModel<InstrumentTradeStockSharpViewModel> res = await DataRepo.InstrumentsSelectAsync(req);
        instruments = res.Response;
    }
}