////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Создает токен сброса пароля для указанного "userId", используя настроенного поставщика токенов сброса пароля.
/// Если "userId" не указан, то команда выполняется для текущего пользователя (запрос/сессия)
/// </summary>
public class GeneratePasswordResetTokenReceive(IIdentityTools idRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<string?, TResponseModel<string?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GeneratePasswordResetTokenReceive;

    /// <summary>
    /// Создает токен сброса пароля для указанного "userId", используя настроенного поставщика токенов сброса пароля.
    /// Если "userId" не указан, то команда выполняется для текущего пользователя (запрос/сессия)
    /// </summary>
    public async Task<TResponseModel<string?>?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, null, req);
        TResponseModel<string?> res = await idRepo.GeneratePasswordResetTokenAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}