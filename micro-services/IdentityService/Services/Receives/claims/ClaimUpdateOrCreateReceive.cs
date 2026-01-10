////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Claim: Update or create
/// </summary>
public class ClaimUpdateOrCreateReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<ClaimUpdateModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ClaimUpdateOrCreateReceive;

    /// <summary>
    /// Claim: Update or create
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ClaimUpdateModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        ResponseBaseModel res = await idRepo.ClaimUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}