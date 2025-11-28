////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// DeliveryStatusDocument
/// </summary>
[Index(nameof(DeliveryStatus)), Index(nameof(Created)), Index(nameof(DeliveryPayment))]
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
    /// Created
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Стоимость доставки
    /// </summary>
    public decimal ShippingCost { get; set; }

    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public DeliveryDocumentModelDB? DeliveryDocument { get; set; }
    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public int DeliveryDocumentId { get; set; }

    /// <summary>
    /// Вес отправления
    /// </summary>
    public decimal WeightShipping { get; set; }
}