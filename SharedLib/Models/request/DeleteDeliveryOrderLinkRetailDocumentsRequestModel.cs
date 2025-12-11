////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeleteDeliveryOrderLinkRetailDocumentsRequestModel
/// </summary>
public class DeleteDeliveryOrderLinkRetailDocumentsRequestModel
{
    /// <inheritdoc/>
    public int OrderDeliveryLinkId { get; set; }

    /// <inheritdoc/>
    public int OrderId { get; set; }

    /// <inheritdoc/>
    public int DeliveryId { get; set; }
}