////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// SetRoleForUserReceive
/// </summary>
public class SetRoleForUserReceive(IIdentityTools identityRepo, ILogger<SetRoleForUserReceive> _logger, IFilesIndexing indexingRepo) 
    : IResponseReceive<SetRoleForUserRequestModel?, TResponseModel<string[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetRoleForUserOfIdentityReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<string[]>?> ResponseHandleActionAsync(SetRoleForUserRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
 TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings)}");
        return await identityRepo.SetRoleForUserAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */