////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeleteRowRetailDocumentRequestModel
/// </summary>
public class DeleteRowRetailDocumentRequestModel
{
    /// <inheritdoc/>
    public int RowId { get; set; }

    /// <inheritdoc/>
    public bool ForceDelete { get; set; }
}