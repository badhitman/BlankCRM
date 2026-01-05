////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class KladrSelectReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrSelectRequestModel?, TPaginationResponseStandardModel<KladrResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<KladrResponseModel>?> ResponseHandleActionAsync(KladrSelectRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectsSelectAsync(req, token);
    }
}