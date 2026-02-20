////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class ТоварВНакладной
{
    /// <remarks/>
    public required string ИдТовара { get; set; }

    /// <remarks/>
    public required string НомерАкцептованногоЗаказа { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public required string УчетныйНомерСертификата { get; set; }

    /// <remarks/>
    public required СтоимостьТип СуммаПоСтроке { get; set; }

    /// <remarks/>
    public DateTime? СрокРеализации { get; set; }

    /// <remarks/>
    public string? НомерГТД { get; set; }

    /// <remarks/>
    public string? Примечание { get; set; }
}