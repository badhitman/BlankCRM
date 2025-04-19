////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharp.Driver;

/// <summary>
/// PingStockSharpDriver
/// </summary>
public class PingStockSharpDriverReceive(IStockSharpDriverService ssRepo)
    : IZeroMQReceive<object?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PingStockSharpDriverReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(object? payload, CancellationToken token = default)
    {
        return await ssRepo.PingAsync(token);
    }
}