////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// EntryUpdatedLiteModel
/// </summary>
[Index(nameof(LastUpdatedAtUTC)), Index(nameof(CreatedAtUTC))]
public class EntryUpdatedLiteModel : EntryModel
{
    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime? LastUpdatedAtUTC { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAtUTC { get; set; }
}