////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет значения свойств номенклатурной позиции в каталоге, пакете предложений, документе
/// </summary>
public partial class ЗначенияСвойства
{
    /// <summary>
    /// Идентификатор свойства в классификаторе товаров
    /// </summary>
    public required string Ид { get; set; }

    /// <summary>
    /// Наименование свойства
    /// </summary>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Значение { get; set; }
}