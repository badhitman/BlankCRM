////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

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