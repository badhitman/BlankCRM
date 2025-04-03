////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CategoriesRusklimatResponseModel
/// </summary>
public class CategoriesRusklimatResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public int TotalCount { get; set; }

    /// <inheritdoc/>
    public CategoryRusklimatModel[]? Data { get; set; }
}