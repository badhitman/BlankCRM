////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class ЭлектроннаяНакладная : КоммерческийДокумент
{
    /// <summary>
    /// Период времени с момента создания документа, в течение которого ожидается реакция на данный документ.
    /// </summary>
    public TimeSpan? ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public required ИдентификаторКонтрагента ИдСклада { get; set; }

    /// <remarks/>
    public string? АдресСклада { get; set; }

    /// <remarks/>
    public required string НомерТоварнойНакладной { get; set; }

    /// <remarks/>
    public DateTime ДатаТоварнойНакладной { get; set; }

    /// <remarks/>
    public required ТоварВНакладной[] Товар { get; set; }
}