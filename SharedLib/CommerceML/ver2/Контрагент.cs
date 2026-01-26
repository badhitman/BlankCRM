namespace SharedLib.CommerceML2;

/// <summary>
/// Универсальное описание контрагента-участника бизнес-процессов.
/// Содержит описание реквизитов юридического или физического лица контрагента.
/// </summary>
public partial class Контрагент
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }

    /// <remarks/>
    public Адрес? Адрес { get; set; }

    /// <remarks/>
    public КонтактнаяИнформация[]? Контакты { get; set; }

    /// <remarks/>
    public Представитель[]? Представители { get; set; }

    /// <remarks/>
    public РеквизитыФизЛица? РеквизитыФизЛица { get; set; }

    /// <remarks/>
    public РеквизитыЮрЛица? РеквизитыЮрЛица { get; set; }
}