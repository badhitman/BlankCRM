////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// GetSumConversionsOrdersAmountsRequest
/// </summary>
public class GetSumConversionsOrdersAmountsRequestModel
{
    /// <inheritdoc/>
    public int[]? ConversionsDocumentsIds { get; set; }

    /// <inheritdoc/>
    public int[]? OrdersDocumentsIds { get; set; }
}