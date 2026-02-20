////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Описывает цену, идентифицированную в каталоге с указанием кода валюты (если ранее не определена)
/// </summary>
public partial class ТипЦены
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Валюта { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <summary>
    /// Определяет вид налога и способ учета налога в цене (сумме)
    /// </summary>
    public Налог[]? Налог { get; set; }
}