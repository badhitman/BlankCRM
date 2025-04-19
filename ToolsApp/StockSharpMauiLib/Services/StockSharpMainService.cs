////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharp.Main;

/// <summary>
/// StockSharpMainService
/// </summary>
public class StockSharpMainService : IStockSharpMainService
{
    public Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ResponseBaseModel.CreateSuccess($"Ok - {nameof(StockSharpMainService)}"));
    }
}