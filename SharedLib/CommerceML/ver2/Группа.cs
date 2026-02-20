////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Описывает группу товаров в каталоге
/// </summary>
public partial class Группа
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <summary>
    /// Содержит коллекцию свойств, значения которых можно или нужно указать для товаров, принадлежащих данной группе, в каталоге, пакете предложений, документах
    /// </summary>
    public Свойство[]? Свойства { get; set; }

    /// <summary>
    /// Содержит описание вложенных групп товаров
    /// </summary>
    public Группа[]? Группы { get; set; }
}