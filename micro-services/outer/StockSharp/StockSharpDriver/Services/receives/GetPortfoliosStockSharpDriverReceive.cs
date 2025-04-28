////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharpDriver;

/// <summary>
/// GetPortfoliosStockSharpDriverReceive
/// </summary>
public class GetPortfoliosStockSharpDriverReceive(IStockSharpDriverService ssRepo)
    : IMQTTReceive<int[], TResponseModel<List<PortfolioStockSharpModel>>>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetPortfoliosStockSharpReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PortfolioStockSharpModel>>> ResponseHandleActionAsync(int[] req, CancellationToken token = default)
    {
        //if (req is null)
        //    throw new ArgumentNullException(nameof(req));

        return await ssRepo.GetPortfoliosAsync(req, token);
    }
}