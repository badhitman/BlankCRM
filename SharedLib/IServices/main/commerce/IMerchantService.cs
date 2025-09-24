////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Linq;

namespace SharedLib;

/// <summary>
/// IMerchantService
/// </summary>
public partial interface IMerchantService
{
    /// <summary>
    /// IncomingTBankMerchantPaymentAsync
    /// </summary>
    public Task<ResponseBaseModel> IncomingTBankMerchantPaymentAsync(JObject req, CancellationToken token = default);

    /// <summary>
    /// BindCustomerTBankAsync
    /// </summary>
    public Task<TResponseModel<UserInfoModel>> BindCustomerTBankAsync(BindCustomerTBankRequestModel req, CancellationToken token = default);

    /// <summary>
    /// InitTBankMerchantPaymentAsync
    /// </summary>
    public Task<TResponseModel<PaymentInitTBankResultModelDB>> InitPaymentMerchantTBankAsync(ReceiptTBankModel req, CancellationToken token = default);
}