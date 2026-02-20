////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Subscribe update - of context user
/// </summary>
public class ExecuterUpdateReceive(IHelpDeskService hdRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<UserIssueModel>?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ExecuterIssueUpdateHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(TAuthRequestStandardModel<UserIssueModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<bool> res = await hdRepo.ExecuterUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}