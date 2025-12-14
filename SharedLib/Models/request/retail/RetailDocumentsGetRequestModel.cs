////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// RetailDocumentsGetRequestModel
/// </summary>
public class RetailDocumentsGetRequestModel
{
    /// <inheritdoc/>
    public required int[] Ids { get; set; }

    /// <inheritdoc/>
    public bool IncludeDataExternal { get; set; }
}