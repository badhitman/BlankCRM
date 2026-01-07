////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Добавить роль пользователю (включить пользователя в роль)
/// </summary>
public class AddRoleToUserReceive(IIdentityTools idRepo, ILogger<AddRoleToUserReceive> loggerRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<RoleEmailModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AddRoleToUserReceive;

    /// <summary>
    /// Добавить роль пользователю (включить пользователя в роль)
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(RoleEmailModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
 TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.AddRoleToUserAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */