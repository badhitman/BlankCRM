////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// SectionOptionFeedItemHaierModel
/// </summary>
public class SectionOptionFeedItemHaierModel
{
    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public required List<OptionFeedItemHaierModel> Options { get; set; }
}