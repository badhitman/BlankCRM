////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// TracesSelectForOrders
/// </summary>
public class TracesSelectForOrdersReceive(ITracesIndexing indexingFileRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectTraceElementsRequestModel>?, TPaginationResponseStandardModel<TraceReceiverRecord>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForOrdersRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectTraceElementsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await indexingFileRepo.TracesSelectForOrdersAsync(req, token);
        return res;
    }
}