////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// RetailDocumentModelDB
/// </summary>
[Index(nameof(BuyerIdentityUserId)), Index(nameof(WarehouseId)), Index(nameof(DateDocument))]
public class RetailDocumentModelDB : OrderDocumentBaseModel
{
    /// <inheritdoc/>
    [Required]
    public required DateTime DateDocument { get; set; }

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
    public List<RetailDeliveryOrderLinkModelDB>? Deliveries { get; set; }

    /// <inheritdoc/>
    public List<RowOfRetailOrderDocumentModelDB>? Rows { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} [{DateDocument}]".Trim();
    }
}