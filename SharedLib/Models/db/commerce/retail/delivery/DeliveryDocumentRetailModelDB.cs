////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// DeliveryDocumentModelDB
/// </summary>
[Index(nameof(DeliveryCode)), Index(nameof(RecipientIdentityUserId)), Index(nameof(DeliveryPaymentUponReceipt))]
[Index(nameof(KladrCode)), Index(nameof(AddressUserComment)), Index(nameof(AuthorIdentityUserId)), Index(nameof(DeliveryStatus))]
[Index(nameof(WarehouseId)), Index(nameof(DeliveryType)), Index(nameof(KladrTitle))]
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

    /// <summary>
    /// Склад списания
    /// </summary>
    public int WarehouseId { get; set; }

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

    /// <inheritdoc/>
    public required string AuthorIdentityUserId { get; set; }

    /// <inheritdoc/>
    public List<RowOfDeliveryRetailDocumentModelDB>? Rows { get; set; }

    /// <summary>
    /// Заказы (документы)
    /// </summary>
    public List<RetailOrderDeliveryLinkModelDB>? OrdersLinks { get; set; }

    /// <summary>
    /// StatusesLog
    /// </summary>
    public List<DeliveryStatusRetailDocumentModelDB>? DeliveryStatusesLog { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        string res = $"[{DeliveryType.DescriptionInfo()}]";

        if (DeliveryStatus is null || DeliveryStatus == 0)
            res = $"{res} (не обработан)";

        if (!string.IsNullOrWhiteSpace(Name))
            res = $"{res} `{Name}`";

        return $"{res}. вес отправления: {WeightShipping}";
    }

    /// <inheritdoc/>
    public static DeliveryDocumentRetailModelDB Build(CreateDeliveryDocumentRetailRequestModel other)
    {
        return new()
        {
            AuthorIdentityUserId = other.AuthorIdentityUserId,
            RecipientIdentityUserId = other.RecipientIdentityUserId,
            AddressUserComment = other.AddressUserComment,
            CreatedAtUTC = other.CreatedAtUTC,
            DeliveryCode = other.DeliveryCode,
            DeliveryPaymentUponReceipt = other.DeliveryPaymentUponReceipt,
            DeliveryStatus = other.DeliveryStatus,
            DeliveryStatusesLog = other.DeliveryStatusesLog,
            DeliveryType = other.DeliveryType,
            Description = other.Description,
            Id = other.Id,
            KladrCode = other.KladrCode,
            KladrTitle = other.KladrTitle,
            LastUpdatedAtUTC = other.LastUpdatedAtUTC,
            Name = other.Name,
            OrdersLinks = other.OrdersLinks,
            Rows = other.Rows,
            ShippingCost = other.ShippingCost,
            WarehouseId = other.WarehouseId,
            WeightShipping = other.WeightShipping,
        };
    }
}