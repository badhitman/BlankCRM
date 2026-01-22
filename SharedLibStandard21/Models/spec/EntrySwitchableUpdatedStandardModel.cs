////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System;

namespace SharedLib;

/// <summary>
/// Базовая DB модель объекта с поддержкой -> int:Id +string:Name +bool:IsDeleted AND UpdatedAt
/// </summary>
public class EntrySwitchableUpdatedStandardModel : IdSwitchableStandardModel
{
    /// <inheritdoc/>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime? LastUpdatedAtUTC { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAtUTC { get; set; }
}