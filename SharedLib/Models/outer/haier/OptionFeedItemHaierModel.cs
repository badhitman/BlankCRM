////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// OptionFeedItemHaierModel
/// </summary>
[Index(nameof(Name)), Index(nameof(Value))]
public class OptionFeedItemHaierModel
{
    /// <inheritdoc/>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public required string Value { get; set; }
}