////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class СвойствоВариантыЗначенийСправочник
{
    /// <summary>
    /// Идентифицирует значение свойства
    /// </summary>
    public required string ИдЗначения { get; set; }

    /// <summary>
    /// Определяет вариант значения свойства
    /// </summary>
    public required string Значение { get; set; }
}