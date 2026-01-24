////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class ТоварАкциз
{
    /// <summary>
    /// Вид акцизного налога.
    /// </summary>
    public required string Наименование { get; set; }

    /// <summary>
    /// Сумма сбора за единицу (базовую) товара
    /// </summary>
    public decimal СуммаЗаЕдиницу { get; set; }

    /// <summary>
    /// Код валюты по международному классификатору валют (ISO 4217).
    /// </summary>
    public required string Валюта { get; set; }
}