////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharpClient;

/// <summary>
/// PingSharpMain
/// </summary>
public class PingSharpMainReceive(IStockSharpMainService ssRepo) 
    : IMQTTReceive<object?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PingStockSharpMainReceive;

    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(object? payload, CancellationToken token = default)
    {
        return await ssRepo.PingAsync(token);
    }
}