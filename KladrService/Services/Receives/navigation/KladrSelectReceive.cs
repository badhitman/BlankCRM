////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrSelectReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrSelectRequestModel?, TPaginationResponseModel<KladrResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.KladrNavigationSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>?> ResponseHandleAction(KladrSelectRequestModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsSelect(req);
    }
}