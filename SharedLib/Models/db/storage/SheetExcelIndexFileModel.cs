////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// SheetExcelIndexFileModel
/// </summary>
[Index(nameof(Title))]
public class SheetExcelIndexFileModel : IndexFileSortedModel
{
    /// <inheritdoc/>
    public required string Title { get; set; }
}