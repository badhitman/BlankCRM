////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// SheetExcelIndexFileModel
/// </summary>
[Index(nameof(Title))]
public class SheetExcelIndexFileModelDB : IndexFileSortedModel
{
    /// <inheritdoc/>
    public required string Title { get; set; }

    /// <inheritdoc/>
    public List<CellTableExcelIndexFileModelDB>? Cells { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{Title}]";
    }
}