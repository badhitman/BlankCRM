////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharpDriver;

/// <summary>
/// GetExchangesStockSharpDriverReceive
/// </summary>
public class GetExchangesStockSharpDriverReceive(IStockSharpDriverService ssRepo)
    : IMQTTReceive<int[], TResponseModel<List<ExchangeStockSharpModel>>>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetExchangesStockSharpReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<ExchangeStockSharpModel>>> ResponseHandleActionAsync(int[] req, CancellationToken token = default)
    {
        //if (req is null)
        //    throw new ArgumentNullException(nameof(req));

        return await ssRepo.GetExchangesAsync(req, token);
    }
}