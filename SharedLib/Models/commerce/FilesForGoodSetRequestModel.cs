////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// FilesForGoodSetRequestModel
/// </summary>
public class FilesForGoodSetRequestModel : FilesForGoodModel
{
    /// <inheritdoc/>
    public IEnumerable<int>? SelectedFiles { get; set; }
}