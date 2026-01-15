////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DeleteDeliveryStatusDocumentResponseModel
/// </summary>
public class DeleteDeliveryStatusDocumentResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public DeliveryStatusRetailDocumentModelDB? DeliveryStatus { get; set; }

    /// <inheritdoc/>
    public DeliveryStatusesEnum? NewStatus { get; set; }

    /// <inheritdoc/>
    public Guid DocumentNewVersion { get; set; }
}