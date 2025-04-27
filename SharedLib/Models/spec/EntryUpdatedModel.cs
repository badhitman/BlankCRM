////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Базовая DB модель объекта с поддержкой -> int:Id +string:Name AND UpdatedAt
/// </summary>
[Index(nameof(LastUpdatedAtUTC))]
public class EntryUpdatedModel : EntryModel
{
    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAtUTC { get; set; }

    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? Description { get; set; }
}