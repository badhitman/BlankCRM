////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Информация о единице товара, присутствующей в заказе
/// </summary>
public partial class СтрокаЗаказа
{
    /// <remarks/>
    public required string ИдТовара { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public required СтоимостьТип Стоимость { get; set; }

    /// <remarks/>
    public string? Примечание { get; set; }
}