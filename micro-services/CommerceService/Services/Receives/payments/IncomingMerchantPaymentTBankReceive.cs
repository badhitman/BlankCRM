////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// IncomingMerchantPaymentTBankReceive
/// </summary>
public class IncomingMerchantPaymentTBankReceive(ICommerceService commerceRepo, ILogger<IncomingMerchantPaymentTBankReceive> loggerRepo)
    : IResponseReceive<IncomingMerchantPaymentTBankNotifyModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IncomingMerchantPaymentTBankReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IncomingMerchantPaymentTBankNotifyModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings)}");
        return await commerceRepo.IncomingMerchantPaymentTBankAsync(req, token);
    }
}