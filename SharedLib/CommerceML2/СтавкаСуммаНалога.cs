namespace SharedLib.CommerceML2;

/// <summary>
/// Вид, ставка и сумма налога.
/// </summary>
public partial class СтавкаСуммаНалога : Налог
{
    /// <remarks/>
    public required decimal Сумма { get; set; }

    /// <remarks/>
    public required string Ставка { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.