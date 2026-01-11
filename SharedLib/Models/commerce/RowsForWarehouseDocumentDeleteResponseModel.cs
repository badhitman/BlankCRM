////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RowsForWarehouseDocumentDeleteResponseModel
/// </summary>
public class RowsForWarehouseDocumentDeleteResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public Dictionary<int, DeliveryDocumentMetadataRecord>? DocumentsUpdated { get; set; }
}

/// <summary>
/// DeliveryDocumentMetadataRecord
/// </summary>
public record struct DeliveryDocumentMetadataRecord(RowOfWarehouseDocumentModelDB[] Rows, Guid VersionDocument);