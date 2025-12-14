////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Строка заказа (документа)
/// </summary>
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