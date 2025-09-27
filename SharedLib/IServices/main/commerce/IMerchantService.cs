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
    /// TBank: зарегистрировать клиента
    /// </summary>
    /// <remarks>
    /// Метод регистрирует клиента в связке с терминалом.
    /// </remarks>
    public Task<TResponseModel<UserInfoModel>> BindCustomerTBankAsync(BindCustomerTBankRequestModel req, CancellationToken token = default);

    /// <summary>
    /// TBank: Инициировать платеж
    /// </summary>
    public Task<TResponseModel<PaymentInitTBankResultModelDB>> InitPaymentMerchantTBankAsync(InitMerchantTBankRequestModel req, CancellationToken token = default);

    /// <summary>
    /// TBank: Обработка входящего платежа (merchant web-hook)
    /// </summary>
    public Task<ResponseBaseModel> IncomingTBankMerchantPaymentAsync(JObject req, CancellationToken token = default);
}