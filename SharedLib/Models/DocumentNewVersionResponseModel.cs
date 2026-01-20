////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DocumentNewVersionResponseModel
/// </summary>
public class DocumentNewVersionResponseModel : TResponseModel<int>
{
    /// <inheritdoc/>
    public Guid? DocumentNewVersion { get; set; }
}