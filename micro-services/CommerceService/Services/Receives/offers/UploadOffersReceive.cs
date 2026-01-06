////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UploadOffersReceive
/// </summary>
public class UploadOffersReceive(ICommerceService commerceRepo)
    : IResponseReceive<List<NomenclatureScopeModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UploadOffersCommerceReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(List<NomenclatureScopeModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        ResponseBaseModel res = await commerceRepo.UploadOffersAsync(req, token);
        return res;
    }
}