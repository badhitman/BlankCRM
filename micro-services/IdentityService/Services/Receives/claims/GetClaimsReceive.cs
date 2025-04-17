////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Get claims
/// </summary>
public class GetClaimsReceive(IIdentityTools idRepo)
    : IResponseReceive<ClaimAreaOwnerModel?, List<ClaimBaseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetClaimsReceive;

    /// <summary>
    /// Get claims
    /// </summary>
    public async Task<List<ClaimBaseModel>?> ResponseHandleActionAsync(ClaimAreaOwnerModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);        
        return await idRepo.GetClaimsAsync(req, token);
    }
}