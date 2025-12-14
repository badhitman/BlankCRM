////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectConversionsOrdersLinksRetailDocumentsRequestModel
/// </summary>
public class SelectConversionsOrdersLinksRetailDocumentsRequestModel
{
    /// <inheritdoc/>
    public int[]? OrdersIds { get; set; }

    /// <inheritdoc/>
    public int[]? ConversionsIds { get; set; }
}