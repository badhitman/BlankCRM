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
public class DocumentRetailModelDB : OrderDocumentBaseModel
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
    public List<RetailOrderDeliveryLinkModelDB>? Deliveries { get; set; }

    /// <inheritdoc/>
    public List<RowOfRetailOrderDocumentModelDB>? Rows { get; set; }

    /// <summary>
    /// Orders
    /// </summary>
    public List<ConversionOrderRetailLinkModelDB>? Conversions { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} [{DateDocument}]".Trim();
    }

    /// <inheritdoc/>
    public static DocumentRetailModelDB Build(CreateDocumentRetailRequestModel other)
    {
        return new()
        {
            AuthorIdentityUserId = other.AuthorIdentityUserId,
            Description = other.Description,
            DateDocument = other.DateDocument,
            BuyerIdentityUserId = other.BuyerIdentityUserId,
            Conversions = other.Conversions,
            CreatedAtUTC = other.CreatedAtUTC,
            Deliveries = other.Deliveries,
            Rows = other.Rows,
            ExternalDocumentId = other.ExternalDocumentId,
            HelpDeskId = other.HelpDeskId,
            Id = other.Id,
            LastUpdatedAtUTC = other.LastUpdatedAtUTC,
            Name = other.Name,
            StatusDocument = other.StatusDocument,
            Version = other.Version,
            WarehouseId = other.WarehouseId,
        };
    }
}