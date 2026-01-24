namespace SharedLib.CommerceML2;

/// <summary>
/// Элементы типа «Товар» - определяют комплектующие составных товаров - наборов.
/// </summary>
public partial class ТоварКомплектующее : Товар
{
    /// <summary>
    /// Идентификатор каталога
    /// </summary>
    public required string ИдКаталога { get; set; }

    /// <summary>
    /// Идентификатор классификатора, в соответствии с которым описано комплектующее
    /// </summary>
    public required string ИдКлассификатора { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.