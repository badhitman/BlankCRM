////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace Transmission.Receives.StockSharpDriver;

/// <summary>
/// InstrumentsSelectStockSharpDriverReceive
/// </summary>
public class InstrumentsSelectStockSharpDriverReceive(IStockSharpDriverService ssRepo)
    : IMQTTReceive<TPaginationRequestStandardModel<InstrumentsRequestModel>, TPaginationResponseModel<InstrumentTradeStockSharpViewModel>>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.InstrumentsSelectStockSharpReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<InstrumentTradeStockSharpViewModel>> ResponseHandleActionAsync(TPaginationRequestStandardModel<InstrumentsRequestModel> req, CancellationToken token = default)
    {
        if (req is null)
            throw new ArgumentNullException(nameof(req));

        return await ssRepo.InstrumentsSelectAsync(req, token);
    }
}