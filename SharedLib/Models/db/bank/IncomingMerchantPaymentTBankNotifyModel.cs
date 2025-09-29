////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IncomingMerchantPaymentTBankNotifyModel
/// </summary>
public class IncomingMerchantPaymentTBankNotifyModel : IncomingMerchantPaymentTBankBaseModel
{
    /// <summary>
    /// Payer
    /// </summary>
    public required string PayerUserId { get; set; }
}