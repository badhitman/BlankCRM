////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrFindReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrFindRequestModel?, TPaginationResponseModel<KladrResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.KladrNavigationFindReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<KladrResponseModel>?> ResponseHandleAction(KladrFindRequestModel? req)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsFind(req);
    }
}