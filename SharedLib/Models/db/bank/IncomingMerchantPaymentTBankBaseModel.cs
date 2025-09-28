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
    public int Id { get; set; }

    /// <inheritdoc/>
    public string? RebillId { get; set; }

    /// <inheritdoc/>
    public decimal? Amount { get; set; }

    /// <inheritdoc/>
    public string? PaymentId { get; set; }

    /// <inheritdoc/>
    public string? Status { get; set; }

    /// <inheritdoc/>
    public int? OrderJoinId { get; set; }

    /// <inheritdoc/>
    public required DateTime CreatedDateTime { get; set; }
}