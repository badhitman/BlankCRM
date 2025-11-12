////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// ParagraphWordIndexFileModel
/// </summary>
[Index(nameof(Data))]
public class ParagraphWordIndexFileModel : IndexFileSortedModel
{
    /// <inheritdoc/>
    public string? Data { get; set; }
}