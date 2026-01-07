////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Получить состояние процедуры привязки аккаунта Telegram к учётной записи сайта (если есть).
/// Если userId не указан, то команда выполняется для текущего пользователя (запрос/сессия)
/// </summary>
public class TelegramJoinAccountStateReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TelegramJoinAccountStateRequestModel?, TResponseModel<TelegramJoinAccountModelDb>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountStateReceive;

    /// <summary>
    /// Получить состояние процедуры привязки аккаунта Telegram к учётной записи сайта (если есть).
    /// Если userId не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>?> ResponseHandleActionAsync(TelegramJoinAccountStateRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.UserId);
        var res = await idRepo.TelegramJoinAccountStateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}