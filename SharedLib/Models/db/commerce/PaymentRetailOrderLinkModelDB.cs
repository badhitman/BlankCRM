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
    /// Заказ (документ)
    /// </summary>
    public RetailDocumentModelDB? Order { get; set; }
    
    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Payment
    /// </summary>
    public PaymentRetailDocumentModelDB? Payment {  get; set; }

    /// <summary>
    /// Payment
    /// </summary>
    public int PaymentId { get; set; }

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