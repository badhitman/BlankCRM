////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Каталог товаров содержит перечень товаров.
/// Может составляться разными предприятиями (например, каталог продукции фирмы «1С»).
/// У каталога всегда определен владелец, а товары могут описываться по классификатору.
/// </summary>
public partial class Каталог
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <summary>
    /// Идентификатор классификатора, в соответствии с которым описываются товары
    /// </summary>
    public string? ИдКлассификатора { get; set; }

    /// <summary>
    /// Элемент типа "Контрагент". Служит для определения владельца данного каталога.
    /// </summary>
    public Контрагент? Владелец { get; set; }

    /// <remarks/>
    public required Товар[] Товары { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public Подписант[]? Подписанты { get; set; }

    /// <remarks/>    
    public bool? СодержитТолькоИзменения { get; set; }
}