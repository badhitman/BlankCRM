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
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.