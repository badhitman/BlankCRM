////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class РеджектЗаказа : КоммерческийДокумент
{
    /// <summary>
    /// Номер исходного заказа клиента
    /// </summary>
    public required string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public TimeSpan? ДлительностьОжиданияРеакции { get; set; }

    /// <remarks/>
    public DateTime ДатаВремяДоставки { get; set; }

    /// <remarks/>
    public TimeSpan? ДлительностьОжиданияДоставки { get; set; }

    /// <summary>
    /// Сумма превышения клиентом кредитного лимита
    /// </summary>
    public СуммаТип? ПревышениеЛимита { get; set; }

    /// <remarks/>
    public required СтрокаЗаказа[] Товар { get; set; }
}