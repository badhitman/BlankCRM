////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UploadOffersReceive
/// </summary>
public class UploadOffersReceive(ICommerceService commerceRepo, ILogger<OfferDeleteReceive> loggerRepo)
    : IResponseReceive<List<NomenclatureScopeModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UploadOffersCommerceReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(List<NomenclatureScopeModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings)}");
        return await commerceRepo.UploadOffersAsync(req, token);
    }
}