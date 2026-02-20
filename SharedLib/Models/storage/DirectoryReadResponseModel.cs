////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// DirectoryReadResponseModel
/// </summary>
public class DirectoryReadResponseModel : DirectoryItemModel
{
    /// <inheritdoc/>
    public List<DirectoryItemModel>? DirectoryItems { get; set; }
}