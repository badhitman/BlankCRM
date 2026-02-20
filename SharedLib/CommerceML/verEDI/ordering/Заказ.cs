////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class Заказ : КоммерческийДокумент
{
    /// <summary>
    /// Период времени с момента создания документа, в течение которого ожидается реакция на данный документ.
    /// </summary>
    public TimeSpan? ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public required ИдентификаторКонтрагента ИдСклада { get; set; }

    /// <remarks/>
    public string? АдресСклада { get; set; }

    /// <summary>
    /// Идентификатор получателя товара
    /// </summary>
    public required ИдентификаторКонтрагента ИдПолучателяТовара { get; set; }

    /// <remarks/>
    public DateTime ДатаВремяДоставки { get; set; }

    /// <remarks/>
    public TimeSpan? ДлительностьОжиданияДоставки { get; set; }

    /// <remarks/>
    public required СтрокаЗаказа[] Товар { get; set; }
}