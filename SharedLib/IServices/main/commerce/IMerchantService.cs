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
    /// IncomingTBankMerchantPayment
    /// </summary>
    public Task<ResponseBaseModel> IncomingTBankMerchantPaymentAsync(JObject req, CancellationToken token = default);
}