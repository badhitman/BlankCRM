////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////


namespace SharedLib;

/// <summary>
/// SelectRetailDocumentsRequestModel
/// </summary>
public class SelectRetailDocumentsBaseModel
{
    /// <inheritdoc/>
    public List<StatusesDocumentsEnum?>? StatusesFilter { get; set; }

    /// <inheritdoc/>
    public DateTime? Start { get; set; }
    /// <inheritdoc/>
    public DateTime? End { get; set; }
}