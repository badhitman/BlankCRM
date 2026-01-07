////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Удалить текущую процедуру привязки Telegram аккаунта к учётной записи сайта
/// </summary>
public class TelegramJoinAccountDeleteActionReceive(IIdentityTools identityRepo, ILogger<TelegramJoinAccountDeleteActionReceive> _logger, IFilesIndexing indexingRepo)
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
 TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings)}");
        return await identityRepo.TelegramJoinAccountDeleteActionAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */