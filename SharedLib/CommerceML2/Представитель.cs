namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class Представитель
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }

    /// <remarks/>
    public string? Отношение { get; set; }

    /// <remarks/>
    public Адрес? Адрес { get; set; }

    /// <remarks/>
    public КонтактнаяИнформация[]? Контакты { get; set; }

    /// <remarks/>
    public РеквизитыФизЛица? РеквизитыФизЛица { get; set; }

    /// <remarks/>
    public РеквизитыЮрЛица? РеквизитыЮрЛица { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.