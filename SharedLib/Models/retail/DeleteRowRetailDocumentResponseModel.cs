////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeleteRowRetailDocumentResponseModel
/// </summary>
public class DeleteRowRetailDocumentResponseModel : TResponseModel<RowOfRetailOrderDocumentModelDB>
{
    /// <inheritdoc/>
    public Guid DocumentNewVersion { get; set; }
}