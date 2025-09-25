////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// PaymentInitTBankResultModelDB
/// </summary>
[Index(nameof(Status)), Index(nameof(PaymentId)), Index(nameof(OrderId)), Index(nameof(Amount))]
public class PaymentInitTBankResultModelDB : PaymentInitTBankResultModel
{
    /// <inheritdoc/>
    public int Id { get; set; }

    /// <inheritdoc/>
    public ReceiptTBankModelDB? Receipt { get; set; }

    /// <inheritdoc/>
    public int ReceiptId { get; set; }

    /// <inheritdoc/>
    public string? ApiException { get; set; }
}