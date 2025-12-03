////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.kladr;

/// <inheritdoc/>
public class ObjectGetReceive(IKladrNavigationService kladrRepo)
    : IResponseReceive<KladrsRequestBaseModel?, TResponseModel<KladrResponseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.KladrNavigationGetObjectReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<KladrResponseModel>?> ResponseHandleActionAsync(KladrsRequestBaseModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await kladrRepo.ObjectGetAsync(req, token);
    }
}