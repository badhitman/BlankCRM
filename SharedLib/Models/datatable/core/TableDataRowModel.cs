////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Модель строки таблицы
/// </summary>
[Index(nameof(IsDisabled))]
public class TableDataRowModel : IdSwitchableStandardModel
{
    /// <summary>
    /// Ячейки строки таблицы
    /// </summary>
    public required IEnumerable<TableDataCellModel> Cells { get; set; }
}