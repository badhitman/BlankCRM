////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DeliveryDocumentModelDB
/// </summary>
[Index(nameof(DeliveryCode)), Index(nameof(DeliveryType)), Index(nameof(RecipientIdentityUserId)), Index(nameof(DeliveryPaymentUponReceipt))]
[Index(nameof(KladrCode)), Index(nameof(KladrTitle)), Index(nameof(AddressUserComment)), Index(nameof(AuthorIdentityUserId)), Index(nameof(DeliveryStatus))]
public class DeliveryDocumentRetailModelDB : EntryUpdatedModel
{
    /// <summary>
    /// DeliveryType
    /// </summary>
    public DeliveryTypesEnum DeliveryType { get; set; }

    /// <summary>
    /// Статус доставки
    /// </summary>
    public DeliveryStatusesEnum? DeliveryStatus { get; set; }

    /// <summary>
    /// Оплата при получении
    /// </summary>
    public bool DeliveryPaymentUponReceipt { get; set; }

    /// <summary>
    /// Получатель
    /// </summary>
    public required string RecipientIdentityUserId { get; set; }

    /// <summary>
    /// Код доставки
    /// </summary>
    public string? DeliveryCode { get; set; }

    /// <summary>
    /// Стоимость доставки
    /// </summary>
    public decimal ShippingCost { get; set; }

    /// <summary>
    /// Вес отправления
    /// </summary>
    public decimal WeightShipping { get; set; }

    #region address
    /// <inheritdoc/>
    public string? KladrCode { get; set; }

    /// <inheritdoc/>
    public string? KladrTitle { get; set; }

    /// <summary>
    /// Адрес 
    /// </summary>
    public string? AddressUserComment { get; set; }
    #endregion

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public RetailDocumentModelDB? Order { get; set; }

    /// <summary>
    /// Заказ (документ)
    /// </summary>
    public int OrderId { get; set; }

    /// <inheritdoc/>
    public required string AuthorIdentityUserId { get; set; }

    /// <summary>
    /// StatusesLog
    /// </summary>
    public List<DeliveryStatusRetailDocumentModelDB>? DeliveryStatusesLog { get; set; }
}