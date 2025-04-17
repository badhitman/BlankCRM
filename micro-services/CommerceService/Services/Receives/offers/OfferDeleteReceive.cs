////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Удалить оффер
/// </summary>
public class OfferDeleteReceive(ICommerceService commerceRepo, ILogger<OfferDeleteReceive> loggerRepo)
    : IResponseReceive<TAuthRequestModel<int>?, ResponseBaseModel?>
{
    /// <summary>
    /// Удалить оффер
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OfferDeleteCommerceReceive;

    /// <summary>
    /// Удалить оффер
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings)}");
        return await commerceRepo.OfferDeleteAsync(req, token);
    }
}