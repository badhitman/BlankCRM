////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Предоставляемая скидка на товарную позицию и/или в целом на сумму документа
/// </summary>
public partial class Скидка
{
    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public string? Процент { get; set; }

    /// <remarks/>
    public bool УчтеноВСумме { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }
}