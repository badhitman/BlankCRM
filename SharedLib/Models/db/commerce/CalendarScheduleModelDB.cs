////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// CalendarScheduleModelDB
/// </summary>
[Index(nameof(DateScheduleCalendar))]
public class CalendarScheduleModelDB : WorkScheduleBaseModelDB
{
    /// <summary>
    /// Дата
    /// </summary>
    public required DateOnly DateScheduleCalendar { get; set; }

    /// <inheritdoc/>
    [Required(AllowEmptyStrings = true, ErrorMessage = "Поле наименования обязательно для заполнения")]
    public override required string Name
    {
        get => $"{base.Name}";
#pragma warning disable CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
        set => base.Name = value;
#pragma warning restore CS8765 // Допустимость значений NULL для типа параметра не соответствует переопределенному элементу (возможно, из-за атрибутов допустимости значений NULL).
    }
}