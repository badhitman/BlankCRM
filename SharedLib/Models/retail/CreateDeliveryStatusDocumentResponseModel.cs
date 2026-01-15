////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CreateDeliveryStatusDocumentResponseModel
/// </summary>
public class CreateDeliveryStatusDocumentResponseModel : TResponseModel<int>
{
    /// <inheritdoc/>
    public Guid DocumentNewVersion { get; set; }
}