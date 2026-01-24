////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Идентификатор склада и количество товаров на этом складе
/// </summary>
public partial class ОстаткиПоСкладам
{
    /// <remarks/>
    public required string ИдСклада { get; set; }

    /// <remarks/>
    public decimal КоличествоНаСкладе { get; set; }
}