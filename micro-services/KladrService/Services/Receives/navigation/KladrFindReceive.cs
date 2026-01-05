////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrFindReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrFindRequestModel?, TPaginationResponseStandardModel<KladrResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationFindReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<KladrResponseModel>?> ResponseHandleActionAsync(KladrFindRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsFindAsync(req, token);
    }
}