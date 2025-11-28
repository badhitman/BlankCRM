////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// DeliveryDocumentModelDB
/// </summary>
[Index(nameof(DeliveryCode))]
public class DeliveryDocumentModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Код доставки
    /// </summary>
    public string? DeliveryCode { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public RetailDocumentModelDB? Order { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// StatusesLog
    /// </summary>
    public List<DeliveryStatusDocumentModelDB>? DeliveryStatusesLog { get; set; }
}