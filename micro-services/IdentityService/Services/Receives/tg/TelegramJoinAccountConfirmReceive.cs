////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// TelegramJoinAccountConfirm receive
/// </summary>
public class TelegramJoinAccountConfirmReceive(IIdentityTools identityRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TelegramJoinAccountConfirmModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountConfirmReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TelegramJoinAccountConfirmModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await identityRepo.TelegramJoinAccountConfirmTokenFromTelegramAsync(req, token: token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}