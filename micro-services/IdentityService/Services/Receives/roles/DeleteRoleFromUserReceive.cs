////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Исключить пользователя из роли (лишить пользователя роли)
/// </summary>
public class DeleteRoleFromUserReceive(IIdentityTools idRepo, ILogger<DeleteRoleFromUserReceive> loggerRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<RoleEmailModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRoleFromUserReceive;

    /// <summary>
    /// Исключить пользователя из роли (лишить пользователя роли)
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(RoleEmailModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
 TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.DeleteRoleFromUserAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */