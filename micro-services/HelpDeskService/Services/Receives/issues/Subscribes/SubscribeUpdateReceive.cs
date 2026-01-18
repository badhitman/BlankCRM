////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// Subscribe update - of context user
/// </summary>
public class SubscribeUpdateReceive(IHelpDeskService hdRepo, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<SubscribeUpdateRequestModel>?, TResponseModel<bool?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SubscribeIssueUpdateHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>?> ResponseHandleActionAsync(TAuthRequestStandardModel<SubscribeUpdateRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<bool?> res = await hdRepo.SubscribeUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}