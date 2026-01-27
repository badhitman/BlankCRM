////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Определяет тип налога, величину ставки налога, сумму налога и включение налога в указанную стоимость
/// </summary>
public partial class СтоимостьНалогТип
{
    /// <remarks/>
    public ТипНалога ТипНалога { get; set; }

    /// <summary>
    /// Выражение величины ставки налога (в %)
    /// </summary>
    public decimal ВеличинаСтавкиНалога { get; set; }

    /// <summary>
    /// Сумма налога, рассчитанная по указанной ставке
    /// </summary>
    public required СуммаТип Сумма { get; set; }

    /// <summary>
    /// Указывает, включен ли данный налог в указанную стоимость
    /// </summary>
    public bool ВключеноВСтоимость { get; set; }
}