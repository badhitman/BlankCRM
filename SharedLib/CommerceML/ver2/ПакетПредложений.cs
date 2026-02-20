////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Содержит перечень коммерческих предложений.
/// Пакет предложений составляется по определенному (только одному) каталогу, а предложения в пакете могут быть описаны по классификатору.
/// </summary>
public partial class ПакетПредложений
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? ИдКаталога { get; set; }

    /// <remarks/>
    public string? ИдКлассификатора { get; set; }

    /// <remarks/>
    public required DateOnly ДействительноС { get; set; }

    /// <remarks/>
    public required DateOnly ДействительноДо { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public Контрагент? Владелец { get; set; }

    /// <summary>
    /// Описывает цены, которые могут быть использованы при формировании пакета коммерческих предложений
    /// </summary>
    public ПакетПредложенийТипыЦен[]? ТипыЦен { get; set; }

    /// <remarks/>
    public Склад[]? Склады { get; set; }

    /// <remarks/>
    public ЗначенияСвойства[]? ЗначенияСвойств { get; set; }

    /// <remarks/>
    public ПакетПредложенийПредложение[]? Предложения { get; set; }

    /// <remarks/>
    public Подписант[]? Подписанты { get; set; }

    /// <remarks/>
    public bool? СодержитТолькоИзменения { get; set; }
}