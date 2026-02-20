////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Дополнительный расход по номенклатурной позиции и/или по документу в целом (например, транспортировка, тара и т.п.)
/// </summary>
public partial class ДопРасход
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