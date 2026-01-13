////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeliveryDocumentMetadataRecord
/// </summary>
public class DeliveryDocumentMetadataRecord
{
    /// <inheritdoc/>
    public required RowOfWarehouseDocumentModelDB[] Rows { get; set; }
    /// <inheritdoc/>
    public required Guid VersionDocument { get; set; }
}