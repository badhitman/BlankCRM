////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DeliveryStatusDocument
/// </summary>
[Index(nameof(DeliveryStatus)), Index(nameof(DeliveryPayment)), Index(nameof(Created)), Index(nameof(Paid))]
public class DeliveryStatusDocumentModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Статус доставки
    /// </summary>
    public DeliveryStatusesEnum DeliveryStatus { get; set; }

    /// <summary>
    /// Способ оплаты доставки
    /// </summary>
    public DeliveryPaymentMethodsEnum DeliveryPayment { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public required DateTime Created { get; set; }

    /// <summary>
    /// Дата оплаты
    /// </summary>
    public DateTime? Paid { get; set; }

    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public DeliveryDocumentModelDB? DeliveryDocument { get; set; }
    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public int DeliveryDocumentId { get; set; }
}