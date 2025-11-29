////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PaymentOrderLinkModelDB
/// </summary>
public class PaymentRetailOrderLinkModelDB : EntryModel
{
    /// <summary>
    /// BuyerIdentityUserId
    /// </summary>
    public required string? PayerIdentityUserId { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public RetailDocumentModelDB? Order { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }

    /// <inheritdoc/>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Comment
    /// </summary>
    public string? Comment { get; set; }
}