////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DeliveryDocumentModelDB
/// </summary>
[Index(nameof(DeliveryCode)), Index(nameof(DeliveryType)), Index(nameof(RecipientIdentityUserId))]
[Index(nameof(KladrCode)), Index(nameof(KladrTitle)), Index(nameof(AddressUserComment))]
public class DeliveryDocumentModelDB
{
    /// <summary>
    /// Идентификатор/Key
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// DeliveryType
    /// </summary>
    public DeliveryTypesEnum DeliveryType { get; set; }

    /// <summary>
    /// Получатель
    /// </summary>
    public string? RecipientIdentityUserId { get; set; }

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
    [Required]
    public required string KladrCode { get; set; }

    /// <inheritdoc/>
    [Required]
    public required string KladrTitle { get; set; }

    /// <summary>
    /// Адрес 
    /// </summary>
    [Required]
    public required string AddressUserComment { get; set; }
    #endregion

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