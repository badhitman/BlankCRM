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
    public async Task<TPaginationResponseModel<KladrResponseModel>?> ResponseHandleActionAsync(KladrSelectRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsSelectAsync(req, token);
    }
}