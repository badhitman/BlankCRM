////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class ТоварАкцептРеджектНакладная
{
    /// <remarks/>
    public required string ИдТовара { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public required СтоимостьТип СуммаПоСтроке { get; set; }

    /// <remarks/>
    public string? Примечание { get; set; }
}