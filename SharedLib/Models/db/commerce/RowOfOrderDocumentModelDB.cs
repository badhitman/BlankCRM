////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// Строка заказа (документа)
/// </summary>
[Index(nameof(OfficeOrderTabId), nameof(OfferId), IsUnique = true)]
[Index(nameof(OfficeOrderTabId))]
public class RowOfOrderDocumentModelDB : RowOfMiddleDocumentModel
{
    /// <summary>
    /// офис доставки (из документа заказа)
    /// </summary>
    public TabOfficeForOrderModelDb? OfficeOrderTab { get; set; }
    /// <summary>
    /// AddressForOrderTab
    /// </summary>
    public int OfficeOrderTabId { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public OrderDocumentModelDB? Order { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// Сумма
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}