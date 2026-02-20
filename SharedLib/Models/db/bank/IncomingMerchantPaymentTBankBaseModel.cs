////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// IncomingMerchantPaymentTBankBaseModel
/// </summary>
public class IncomingMerchantPaymentTBankBaseModel
{
    /// <inheritdoc/>
    public string? RebillId { get; set; }

    /// <inheritdoc/>
    public decimal? Amount { get; set; }

    /// <inheritdoc/>
    public string? PaymentId { get; set; }

    /// <inheritdoc/>
    public string? Status { get; set; }
}