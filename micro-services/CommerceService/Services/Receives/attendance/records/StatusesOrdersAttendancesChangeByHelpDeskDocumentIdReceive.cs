////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// StatusesOrdersAttendancesChangeByHelpDeskDocumentId
/// </summary>
public class StatusesOrdersAttendancesChangeByHelpDeskDocumentIdReceive(ICommerceService commRepo, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<StatusChangeRequestModel>?, TResponseModel<List<RecordsAttendanceModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.StatusesOrdersAttendancesChangeByHelpDeskDocumentIdReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RecordsAttendanceModelDB>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<StatusChangeRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<List<RecordsAttendanceModelDB>> res = await commRepo.StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}