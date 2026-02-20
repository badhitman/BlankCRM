////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public abstract partial class КоммерческийДокумент
{
    /// <remarks/>
    public required ИдентификаторКонтрагента ИдОтправителя { get; set; }

    /// <remarks/>
    public required ИдентификаторКонтрагента ИдПолучателя { get; set; }

    /// <remarks/>
    public required string НомерДокумента { get; set; }

    /// <remarks/>
    public required DateTime МоментСоздания { get; set; }

    /// <remarks/>
    public string? Примечание { get; set; }
}