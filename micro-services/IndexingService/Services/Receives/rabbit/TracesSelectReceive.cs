////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// TracesSelect
/// </summary>
public class TracesSelectReceive(IHistoryIndexing indexingFileRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectHistoryReceivesRequestModel>?, TPaginationResponseStandardModel<TraceReceiverRecord>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectHistoryReceivesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TPaginationResponseStandardModel<TraceReceiverRecord> res = await indexingFileRepo.SelectHistoryBaseAsync(req, token);
        return res;
    }
}