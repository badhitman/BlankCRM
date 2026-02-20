////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class ЗапросПодтверждениеНакладной : КоммерческийДокумент
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
    public required ТоварВНакладной[] Товар { get; set; }
}