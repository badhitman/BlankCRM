////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ProductsRusklimatResponseModel
/// </summary>
public class ProductsRusklimatResponseModel : ResponseBaseModel
{
    /// <inheritdoc/>
    public int TotalCount { get; set; }

    /// <inheritdoc/>
    public int PageSize { get; set; }

    /// <inheritdoc/>
    public int Page { get; set; }

    /// <inheritdoc/>
    public int TotalPageCount { get; set; }

    /// <inheritdoc/>
    public ProductRusklimatModel[]? Data { get; set; }
}