////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.FilesIndexing;

public partial class SheetViewComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required List<CellTableExcelIndexFileModelDB> CellsData { get; set; }

    uint maxColIndex, maxRowIndex;

    string? GetData(uint _c, uint _r) 
        => CellsData.FirstOrDefault(x => x.ColNum == _c && x.RowNum == _r)?.Data;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (CellsData.Count != 0)
        {
            maxColIndex = CellsData.Max(x => x.ColNum);
            maxRowIndex = CellsData.Max(x => x.RowNum);
        }
    }
}