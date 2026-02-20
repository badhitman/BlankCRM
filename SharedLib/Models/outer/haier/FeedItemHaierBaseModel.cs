////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// FeedItemHaierBaseModel
/// </summary>
[Index(nameof(Price)), Index(nameof(Category)), Index(nameof(ParentCategory)), Index(nameof(AllArticles))]
public class FeedItemHaierBaseModel
{
    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public required string Url { get; set; }

    /// <inheritdoc/>
    public required string Price { get; set; }

    /// <inheritdoc/>
    public required string Category { get; set; }

    /// <inheritdoc/>
    public string? ParentCategory { get; set; }

    /// <inheritdoc/>
    public string? AllArticles { get; set; }

    /// <inheritdoc/>
    public string? ImageLink { get; set; }

    /// <inheritdoc/>
    public required string Description { get; set; }
}