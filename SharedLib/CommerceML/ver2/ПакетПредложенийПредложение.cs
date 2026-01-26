////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class ПакетПредложенийПредложение : Товар
{
    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public Цена[]? Цены { get; set; }

    /// <remarks/>
    public ОстаткиПоСкладам[]? Склад { get; set; }
}