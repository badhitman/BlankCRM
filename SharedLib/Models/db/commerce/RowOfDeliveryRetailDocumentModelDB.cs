////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Строка заказа (документа)
/// </summary>
public class RowOfDeliveryRetailDocumentModelDB : RowOfRetailOrderDocumentBaseModelDB
{
    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public DeliveryDocumentRetailModelDB? Document { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int DocumentId { get; set; }
}