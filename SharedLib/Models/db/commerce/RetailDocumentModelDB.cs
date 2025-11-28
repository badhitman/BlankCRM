////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// RetailDocumentModelDB
/// </summary>
[Index(nameof(PayerIdentityUserId)), Index(nameof(BuyerIdentityUserId)), Index(nameof(WarehouseId))]
[Index(nameof(DeliveryType))]
public class RetailDocumentModelDB : OrderDocumentBaseModel
{
    /// <summary>
    /// BuyerIdentityUserId
    /// </summary>
    public required string BuyerIdentityUserId { get; set; }

    /// <summary>
    /// BuyerIdentityUserId
    /// </summary>
    public required string? PayerIdentityUserId { get; set; }

    /// <summary>
    /// Склад
    /// </summary>
    public int WarehouseId { get; set; }

    /// <summary>
    /// DeliveryType
    /// </summary>
    public DeliveryTypesEnum DeliveryType { get; set; }

    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public List<DeliveryDocumentModelDB>? DeliveryDocument { get; set; }

    /// <summary>
    /// Rows
    /// </summary>
    public List<RowOfRetailOrderDocumentModelDB>? Rows { get; set; }
}