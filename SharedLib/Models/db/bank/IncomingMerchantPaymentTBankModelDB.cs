////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// IncomingMerchantPaymentModelDB
/// </summary>
[Index(nameof(PaymentId)), Index(nameof(Amount)), Index(nameof(RebillId)), Index(nameof(CardId)), Index(nameof(ExpDate))]
[Index(nameof(Status)), Index(nameof(OrderId)), Index(nameof(OrderJoinId))]
public class IncomingMerchantPaymentTBankModelDB : IncomingMerchantPaymentTBankBaseModel
{
    /// <inheritdoc/>
    public string? ExpDate { get; set; }

    /// <inheritdoc/>
    public string? Pan { get; set; }

    /// <inheritdoc/>
    public string? CardId { get; set; }

    /// <inheritdoc/>
    public string? OrderId { get; set; }
}