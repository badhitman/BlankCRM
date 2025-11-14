////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// CellTableAbstractIndexFileModel
/// </summary>
[Index(nameof(Data))]
public abstract class CellTableAbstractIndexFileModel : IndexFileBaseModel
{
    /// <inheritdoc/>
    public uint RowNum { get; set; }

    /// <inheritdoc/>
    public uint ColNum { get; set; }

    /// <inheritdoc/>
    public required string Data { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[r:{RowNum};c:{ColNum};] {Data}";
    }
}