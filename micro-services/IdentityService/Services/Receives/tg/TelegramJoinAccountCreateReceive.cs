////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Инициировать новую процедуру привязки Telegram аккаунта к учётной записи сайта
/// </summary>
public class TelegramJoinAccountCreateReceive(IIdentityTools identityRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<string?, TResponseModel<TelegramJoinAccountModelDb>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountCreateReceive;

    /// <summary>
    /// Инициировать новую процедуру привязки Telegram аккаунта к учётной записи сайта
    /// </summary>
    public async Task<TResponseModel<TelegramJoinAccountModelDb>?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<TelegramJoinAccountModelDb> res = await identityRepo.TelegramJoinAccountCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}