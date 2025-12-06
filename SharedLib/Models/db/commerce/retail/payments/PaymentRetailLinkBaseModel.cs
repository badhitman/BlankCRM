////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// PaymentRetailLinkBaseModel
/// </summary>
public class PaymentRetailLinkBaseModel : EntryModel
{
    /// <summary>
    /// Payment
    /// </summary>
    public PaymentRetailDocumentModelDB? Payment { get; set; }

    /// <summary>
    /// Payment
    /// </summary>
    public int PaymentId { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }
}