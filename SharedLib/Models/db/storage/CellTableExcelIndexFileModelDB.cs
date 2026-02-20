////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CellTableExcelIndexFileModel
/// </summary>
public class CellTableExcelIndexFileModelDB : CellTableAbstractIndexFileModel
{
    /// <summary>
    /// SheetExcelFile
    /// </summary>
    public SheetExcelIndexFileModelDB? SheetExcelFile { get; set; }

    /// <summary>
    /// SheetExcelFile
    /// </summary>
    public int SheetExcelFileId { get; set; }
}