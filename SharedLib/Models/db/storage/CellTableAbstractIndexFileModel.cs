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
    public string? Data { get; set; }
}