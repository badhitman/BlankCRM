////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Строка прайс-листа
/// </summary>
public partial class ТоварПрайсЛист
{
    /// <remarks/>
    public required string ИдТовара { get; set; }

    /// <remarks/>
    public required СтоимостьТип Цена { get; set; }

    /// <remarks/>
    public string? Примечание { get; set; }
}