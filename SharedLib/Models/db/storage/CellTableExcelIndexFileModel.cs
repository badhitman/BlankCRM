////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CellTableExcelIndexFileModel
/// </summary>
public class CellTableExcelIndexFileModel : CellTableAbstractIndexFileModel
{
    /// <summary>
    /// SheetExcelFile
    /// </summary>
    public SheetExcelIndexFileModel? SheetExcelFile { get; set; }

    /// <summary>
    /// SheetExcelFile
    /// </summary>
    public int SheetExcelFileId { get; set; }
}