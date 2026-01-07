////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Удалить роль (если у роли нет пользователей).
/// </summary>
public class DeleteRoleReceive(IIdentityTools idRepo, ILogger<DeleteRoleReceive> loggerRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<string?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRoleReceive;

    /// <summary>
    /// Добавить роль пользователю (включить пользователя в роль)
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        if(string.IsNullOrWhiteSpace(req))
            throw new ArgumentNullException(nameof(req));
 TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.DeleteRoleAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */