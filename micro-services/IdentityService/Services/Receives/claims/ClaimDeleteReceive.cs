////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Claim: Remove
/// </summary>
public class ClaimDeleteReceive(IIdentityTools idRepo, ILogger<ClaimDeleteReceive> loggerRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<ClaimAreaIdModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ClaimDeleteReceive;

    /// <summary>
    /// Claim: Remove
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ClaimAreaIdModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
 TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.ClaimDeleteAsync(req, token);
    }
}
/*
        
await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
 */