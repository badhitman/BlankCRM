////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.FilesIndexing;

/// <summary>
/// TableWordViewComponent
/// </summary>
public partial class TableWordViewComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required List<CellTableWordIndexFileModelDB> TableData { get; set; }

    uint maxColIndex, maxRowIndex;

    string? GetData(uint _c, uint _r)
        => TableData.FirstOrDefault(x => x.ColNum == _c && x.RowNum == _r)?.Data;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (TableData.Count != 0)
        {
            maxColIndex = TableData.Max(x => x.ColNum);
            maxRowIndex = TableData.Max(x => x.RowNum);
        }
    }
}