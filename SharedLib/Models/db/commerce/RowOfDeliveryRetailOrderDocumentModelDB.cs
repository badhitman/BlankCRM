////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Строка заказа (документа)
/// </summary>
public class RowOfDeliveryRetailOrderDocumentModelDB : RowOfRetailOrderDocumentBaseModelDB
{
    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public DeliveryDocumentModelDB? Document { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int DocumentId { get; set; }
}