////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// MerchantTransmission
/// </summary>
public partial class MerchantTransmission(IRabbitClient rabbitClient) : IMerchantService
{
    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel>> BindCustomerTBankAsync(BindCustomerTBankRequestModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserInfoModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.BindCustomerTBankReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IncomingTBankMerchantPaymentAsync(JObject req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.IncomingTBankMerchantPaymentReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentInitTBankResultModelDB>> InitPaymentMerchantTBankAsync(ReceiptTBankModel req, CancellationToken token = default)
       => await rabbitClient.MqRemoteCallAsync<TResponseModel<PaymentInitTBankResultModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.InitPaymentMerchantTBankReceive, req, token: token) ?? new();
}