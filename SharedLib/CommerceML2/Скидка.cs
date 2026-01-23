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
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.