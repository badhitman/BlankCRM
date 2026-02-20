////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// SetRoleForUser
/// </summary>
public class SetRoleForUserReceive(IIdentityTools identityRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<SetRoleForUserRequestModel>?, TResponseModel<string[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetRoleForUserOfIdentityReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<string[]>?> ResponseHandleActionAsync(TAuthRequestStandardModel<SetRoleForUserRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<string[]> res = await identityRepo.SetRoleForUserAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}