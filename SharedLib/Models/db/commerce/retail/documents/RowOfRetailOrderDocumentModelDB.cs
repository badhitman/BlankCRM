////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Строка заказа (документа)
/// </summary>
[Index(nameof(OrderId), nameof(OfferId), IsUnique = true)]
public class RowOfRetailOrderDocumentModelDB : RowOfRetailOrderDocumentBaseModelDB
{
    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public DocumentRetailModelDB? Order { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int OrderId { get; set; }
}