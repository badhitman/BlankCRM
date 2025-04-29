////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.helpdesk;

/// <summary>
/// MessageVote Receive
/// </summary>
public class MessageVoteReceive(IHelpDeskService hdRepo) : IResponseReceive<TAuthRequestModel<VoteIssueRequestModel>?, TResponseModel<bool?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MessageOfIssueVoteHelpDeskReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool?>?> ResponseHandleActionAsync(TAuthRequestModel<VoteIssueRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await hdRepo.MessageVoteAsync(req, token);
    }
}