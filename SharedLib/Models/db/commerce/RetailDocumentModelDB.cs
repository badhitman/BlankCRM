////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// RetailDocumentModelDB
/// </summary>
[Index(nameof(BuyerIdentityUserId)), Index(nameof(WarehouseId))]
public class RetailDocumentModelDB : OrderDocumentBaseModel
{
    /// <summary>
    /// BuyerIdentityUserId
    /// </summary>
    public required string BuyerIdentityUserId { get; set; }

    /// <summary>
    /// Склад
    /// </summary>
    public int WarehouseId { get; set; }

    /// <inheritdoc/>
    public string? Description { get; set; }

    /// <summary>
    /// DeliveryDocument
    /// </summary>
    public List<DeliveryDocumentRetailModelDB>? DeliveryDocuments { get; set; }

    /// <summary>
    /// Rows
    /// </summary>
    public List<RowOfRetailOrderDocumentModelDB>? Rows { get; set; }

    /// <summary>
    /// PaymentsLinks
    /// </summary>
    public List<PaymentRetailOrderLinkModelDB>? PaymentsLinks { get; set; }
}