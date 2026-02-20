////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Базовая DB модель объекта с поддержкой -> int:Id +string:Name +string:Description +bool:IsDeleted
/// </summary>
[Index(nameof(Name)), Index(nameof(IsDisabled))]
public class EntryDescriptionSwitchableModel : EntrySwitchableModel
{
    /// <summary>
    /// Описание/примечание для объекта
    /// </summary>
    public string? Description { get; set; }


    /// <inheritdoc/>
    public static new EntryDescriptionSwitchableModel Build(string name)
        => new() { Name = name };

    /// <inheritdoc/>
    public static EntryDescriptionSwitchableModel Build(string name, string description)
        => new() { Name = name, Description = description };
}