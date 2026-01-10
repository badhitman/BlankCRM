////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Удалить текущую процедуру привязки Telegram аккаунта к учётной записи сайта
/// </summary>
public class TelegramJoinAccountDeleteActionReceive(IIdentityTools identityRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<string?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountDeleteActionReceive;

    /// <summary>
    /// Удалить текущую процедуру привязки Telegram аккаунта к учётной записи сайта
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        ResponseBaseModel res = await identityRepo.TelegramJoinAccountDeleteActionAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}