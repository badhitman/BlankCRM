namespace SharedLib.CommerceML2;

/// <summary>
/// Описывает классификацию товаров.
/// </summary>
public partial class Классификатор
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public required Контрагент Владелец { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public required Группа[] Группы { get; set; }

    /// <remarks/>
    public required Свойство[] Свойства { get; set; }

    /// <remarks/>
    public required ТипЦены[] ТипыЦен { get; set; }

    /// <remarks/>
    public required Подписант[] Подписанты { get; set; }
}