////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Удалить связь Telegram аккаунта с учётной записью сайта
/// </summary>
public class TelegramAccountRemoveIdentityJoinReceive(IIdentityTools identityRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TelegramAccountRemoveJoinRequestIdentityModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TelegramAccountRemoveIdentityJoinReceive;

    /// <summary>
    /// Удалить связь Telegram аккаунта с учётной записью сайта
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TelegramAccountRemoveJoinRequestIdentityModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await identityRepo.TelegramAccountRemoveIdentityJoinAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}