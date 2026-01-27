////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class КаталогТоваров : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <summary>
    /// Период времени с момента создания документа, в течение которого ожидается реакция на данный документ.
    /// </summary>
    public TimeSpan? ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public bool? ЭтоПолныйКаталог { get; set; }

    /// <remarks/>
    public required СтрокаКаталога[] Товар { get; set; }
}
