////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// StatusChangeReceive
/// </summary>
public class StatusIssueChangeReceive(IHelpDeskService hdRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<StatusChangeRequestModel>?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.StatusChangeIssueHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(TAuthRequestStandardModel<StatusChangeRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<bool> res = await hdRepo.IssueStatusChangeAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}