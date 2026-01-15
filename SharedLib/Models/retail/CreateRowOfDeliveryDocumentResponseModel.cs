////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CreateRowOfDeliveryDocumentResponseModel
/// </summary>
public class CreateRowOfDeliveryDocumentResponseModel : TResponseModel<int>
{
    /// <inheritdoc/>
    public Guid DocumentNewVersion { get; set; }
}