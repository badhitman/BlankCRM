////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharpDriver;

/// <summary>
/// InstrumentFavoriteToggleReceive
/// </summary>
public class InstrumentFavoriteToggleReceive(IStockSharpDriverService ssRepo)
    : IMQTTReceive<InstrumentTradeStockSharpViewModel, ResponseBaseModel>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentFavoriteToggleStockSharpReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> ResponseHandleActionAsync(InstrumentTradeStockSharpViewModel req, CancellationToken token = default)
    {
        if (req is null)
            throw new ArgumentNullException(nameof(req));

        return await ssRepo.InstrumentFavoriteToggleAsync(req, token);
    }
}