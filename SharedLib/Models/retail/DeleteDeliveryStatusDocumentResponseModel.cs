////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json.Converters;

namespace SharedLib;

/// <summary>
/// DeleteDeliveryStatusDocumentResponseModel
/// </summary>
public class DeleteDeliveryStatusDocumentResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public DeliveryStatusRetailDocumentModelDB? DeliveryStatus { get; set; }

    /// <inheritdoc/>
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public DeliveryStatusesEnum? NewStatus { get; set; }

    /// <inheritdoc/>
    public Guid DocumentNewVersion { get; set; }
}