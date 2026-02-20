////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет значение произвольного реквизита документа
/// </summary>
public partial class ЗначениеРеквизита
{
    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    [StringLength(1000)]
    public string? Значение { get; set; }
}