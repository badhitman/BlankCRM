////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// TracesSelectForDeliveriesRetail
/// </summary>
public class TracesSelectForDeliveriesRetailReceive(IHistoryIndexing indexingFileRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectTraceElementsRequestModel>?, TPaginationResponseStandardModel<TraceReceiverRecord>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForDeliveriesRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await indexingFileRepo.SelectHistoryForDeliveriesRetailAsync(req, token);
        return res;
    }
}