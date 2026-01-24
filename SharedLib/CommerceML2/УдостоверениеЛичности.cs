////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class УдостоверениеЛичности
{
    /// <remarks/>
    public required string ВидДокумента { get; set; }

    /// <remarks/>
    public required string Серия { get; set; }

    /// <remarks/>
    public required string Номер { get; set; }

    /// <remarks/>
    public DateOnly ДатаВыдачи { get; set; }

    /// <remarks/>
    public required string КемВыдан { get; set; }
}