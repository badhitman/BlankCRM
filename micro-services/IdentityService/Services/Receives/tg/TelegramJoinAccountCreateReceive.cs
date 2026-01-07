////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Инициировать новую процедуру привязки Telegram аккаунта к учётной записи сайта
/// </summary>
public class TelegramJoinAccountCreateReceive(IIdentityTools identityRepo, ILogger<TelegramJoinAccountCreateReceive> _logger, IFilesIndexing indexingRepo)
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
        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings)}");
        return await identityRepo.TelegramJoinAccountCreateAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */