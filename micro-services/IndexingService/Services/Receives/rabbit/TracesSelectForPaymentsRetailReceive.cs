////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// TracesSelectForPaymentsRetail
/// </summary>
public class TracesSelectForPaymentsRetailReceive(IHistoryIndexing indexingFileRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>?, TPaginationResponseStandardModel<TraceReceiverRecord>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForPaymentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await indexingFileRepo.SelectHistoryForPaymentsRetailAsync(req, token);
        return res;
    }
}