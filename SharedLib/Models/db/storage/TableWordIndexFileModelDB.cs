////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TableWordIndexFileModel
/// </summary>
public class TableWordIndexFileModelDB : IndexFileSortedModel
{
    /// <inheritdoc/>
    public List<CellTableWordIndexFileModelDB>? Data { get; set; }
}