////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeliveryDocumentMetadataModel
/// </summary>
public class DeliveryDocumentMetadataModel
{
    /// <inheritdoc/>
    public required RowOfWarehouseDocumentModelDB[] Rows { get; set; }
    /// <inheritdoc/>
    public required Guid VersionDocument { get; set; }
}