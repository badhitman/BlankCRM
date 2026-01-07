////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// InitChangePhoneUser
/// </summary>
public class InitChangePhoneUserReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<string>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.InitChangePhoneUserReceive;

    /// <summary>
    /// Добавляет password к указанному userId, только если у пользователя еще нет пароля.
    /// Если userId не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<string>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.SenderActionUserId);
        ResponseBaseModel res = await idRepo.InitChangePhoneUserAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}