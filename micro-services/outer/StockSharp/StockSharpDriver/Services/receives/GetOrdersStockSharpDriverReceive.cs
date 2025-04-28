////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharpDriver;

/// <summary>
/// GetOrdersStockSharpDriverReceive
/// </summary>
public class GetOrdersStockSharpDriverReceive(IStockSharpDriverService ssRepo)
    : IMQTTReceive<int[], TResponseModel<List<OrderStockSharpModel>>>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetOrdersStockSharpReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<OrderStockSharpModel>>> ResponseHandleActionAsync(int[] req, CancellationToken token = default)
    {
        //if (req is null)
        //    throw new ArgumentNullException(nameof(req));

        return await ssRepo.GetOrdersAsync(req, token);
    }
}