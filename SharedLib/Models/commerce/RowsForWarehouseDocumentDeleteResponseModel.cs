////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RowsForWarehouseDocumentDeleteResponseModel
/// </summary>
public class RowsForWarehouseDocumentDeleteResponseModel : TResponseModel<bool>
{
    /// <inheritdoc/>
    public Dictionary<int, int[]>? DocumentsUpdated { get; set; }
}