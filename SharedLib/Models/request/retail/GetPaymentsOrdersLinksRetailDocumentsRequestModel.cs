////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// GetPaymentsOrdersLinksRetailDocumentsRequestModel
/// </summary>
public class GetPaymentsOrdersLinksRetailDocumentsRequestModel
{
    /// <inheritdoc/>
    public int[]? LinksIds { get; set; }

    /// <inheritdoc/>
    public int[]? OrdersIds { get; set; }
}