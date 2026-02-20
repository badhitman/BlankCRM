////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// CreateUserManual
/// </summary>
public class CreateUserManualReceive(IIdentityTools idRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<UserInfoBaseModel>?, TResponseModel<string>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateManualUserReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<string>?> ResponseHandleActionAsync(TAuthRequestStandardModel<UserInfoBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<string> res = await idRepo.CreateUserManualAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}