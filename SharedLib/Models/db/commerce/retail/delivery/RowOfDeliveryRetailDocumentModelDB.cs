////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Строка заказа (документа)
/// </summary>
[Index(nameof(DocumentId), nameof(OfferId), IsUnique = true)]
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