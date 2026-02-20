////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// TracesSelectForConversionsRetail
/// </summary>
public class TracesSelectForConversionsRetailReceive(IHistoryIndexing indexingFileRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>?, TPaginationResponseStandardModel<TraceReceiverRecord>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectForConversionsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectHistoryElementsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await indexingFileRepo.SelectHistoryForConversionsRetailAsync(req, token);
        return res;
    }
}