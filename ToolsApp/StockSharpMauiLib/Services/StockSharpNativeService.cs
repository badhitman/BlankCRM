////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace StockSharpService;

/// <summary>
/// StockSharpNativeService
/// </summary>
public class StockSharpNativeService : IStockSharpDriverService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ResponseBaseModel.CreateSuccess($"Ok - {nameof(StockSharpNativeService)}"));
    }
}