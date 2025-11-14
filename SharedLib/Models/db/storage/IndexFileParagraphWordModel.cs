////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// ParagraphWordIndexFileModel
/// </summary>
[Index(nameof(Data))]
public class ParagraphWordIndexFileModelDB : IndexFileSortedModel
{
    /// <inheritdoc/>
    public string? Data { get; set; }

    /// <inheritdoc/>
    public string? ParagraphId { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[#{ParagraphId}]{Data}";
    }
}