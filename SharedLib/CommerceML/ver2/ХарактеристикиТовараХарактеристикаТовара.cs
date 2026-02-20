////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Уточняет характеристики поставляемого товара. Товар с разными характеристиками может иметь разную цену
/// </summary>
public partial class ХарактеристикиТовараХарактеристикаТовара
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public required string Значение { get; set; }
}