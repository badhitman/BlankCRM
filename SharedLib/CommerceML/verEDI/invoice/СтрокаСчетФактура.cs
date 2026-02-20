////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class СтрокаСчетФактура
{
    /// <remarks/>
    public required string ИдНакладной { get; set; }

    /// <remarks/>
    public required string ИдТовара { get; set; }

    /// <summary>
    /// Представление Кода по Общероссийскому классификатору единиц измерения ОКЕИ
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{3}
    /// </remarks>
    public string? ОКЕИ { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public required СтоимостьТип Стоимость { get; set; }
}