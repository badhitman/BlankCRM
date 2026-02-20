////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// CellTableWordIndexFileModel
/// </summary>
public class CellTableWordIndexFileModelDB : CellTableAbstractIndexFileModel
{
    /// <summary>
    /// TableWordFile
    /// </summary>
    public TableWordIndexFileModelDB? TableWordFile { get; set; }

    /// <summary>
    /// TableWordFile
    /// </summary>
    public int TableWordFileId { get; set; }
    
    /// <inheritdoc/>
    public string? ParagraphId { get; set; }

}