////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class СчетФактура : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерСчетФактураПоставщик { get; set; }

    /// <remarks/>
    public required DateTime ДатаСчетФактураПоставщик { get; set; }

    /// <summary>
    /// Период времени с момента создания документа, в течение которого ожидается реакция на данный документ.
    /// </summary>
    public TimeSpan? ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public required СтрокаСчетФактура[] СтрокаСчетФактура { get; set; }
}