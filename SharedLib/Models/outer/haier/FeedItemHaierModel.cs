////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// FeedItemHaierModel
/// </summary>
public class FeedItemHaierModel: FeedItemHaierBaseModel
{
    /// <inheritdoc/>
    public List<SectionOptionFeedItemHaierModel>? Options { get; set; }

    /// <inheritdoc/>
    public List<FileFeedItemHaierModel>? Files { get; set; }
}