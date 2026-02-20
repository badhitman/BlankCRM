////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// IndexFileSortedModel
/// </summary>
[Index(nameof(SortIndex))]
public class IndexFileSortedModel : IndexFileBaseModel
{
    /// <inheritdoc/>
    public int SortIndex { get; set; }
}