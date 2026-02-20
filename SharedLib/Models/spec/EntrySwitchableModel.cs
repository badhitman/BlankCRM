////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SharedLib;

/// <summary>
/// Базовая DB модель объекта с поддержкой -> int:Id +string:Name +bool:IsDeleted
/// </summary>
[Index(nameof(Name)), Index(nameof(IsDisabled))]
public class EntrySwitchableModel : IdSwitchableStandardModel
{
    /// <inheritdoc/>
    public static EntryStandardModel Build(string name) => new() { Name = name };

    /// <summary>
    /// Имя объекта
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Поле наименования обязательно для заполнения")]
    public virtual required string Name { get; set; }
}