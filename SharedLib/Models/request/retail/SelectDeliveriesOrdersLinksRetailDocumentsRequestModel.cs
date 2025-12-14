////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SelectDeliveriesOrdersLinksRetailDocumentsRequestModel
/// </summary>
public class SelectDeliveriesOrdersLinksRetailDocumentsRequestModel
{
    /// <inheritdoc/>
    public int[]? OrdersIds { get; set; }

    /// <inheritdoc/>
    public int[]? DeliveriesIds { get; set; }
}