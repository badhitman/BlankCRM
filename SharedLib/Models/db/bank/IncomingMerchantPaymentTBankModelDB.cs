////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// IncomingMerchantPaymentModelDB
/// </summary>
[Index(nameof(Status)), Index(nameof(OrderId)), Index(nameof(PaymentId)), Index(nameof(Amount)), Index(nameof(RebillId)), Index(nameof(CardId)), Index(nameof(ExpDate))]
public class IncomingMerchantPaymentTBankModelDB
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public string? ExpDate { get; set; }

    /// <inheritdoc/>
    public string? Pan { get; set; }

    /// <inheritdoc/>
    public string? CardId { get; set; }

    /// <inheritdoc/>
    public string? RebillId { get; set; }

    /// <inheritdoc/>
    public decimal? Amount { get; set; }

    /// <inheritdoc/>
    public string? PaymentId { get; set; }

    /// <inheritdoc/>
    public string? OrderId { get; set; }

    /// <inheritdoc/>
    public string? Status { get; set; }

    /// <inheritdoc/>
    public required DateTime CreatedDateTime { get; set; }
}