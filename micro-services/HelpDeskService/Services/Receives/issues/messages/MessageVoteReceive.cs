////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// MessageVote Receive
/// </summary>
public class MessageVoteReceive(IHelpDeskService hdRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<VoteIssueRequestModel>?, TResponseModel<bool?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueVoteHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>?> ResponseHandleActionAsync(TAuthRequestStandardModel<VoteIssueRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload.MessageId);
        TResponseModel<bool?> res = await hdRepo.MessageVoteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}