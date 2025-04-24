////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace StockSharpDriver;

/// <summary>
/// StockSharpDriverService
/// </summary>
public class StockSharpDriverService : IStockSharpDriverService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> PingAsync(CancellationToken cancellationToken = default)
    {
        StockSharp.Algo.Connector Connector = new();
        return Task.FromResult(ResponseBaseModel.CreateSuccess($"Ok - {nameof(StockSharpDriverService)}"));
    }
}