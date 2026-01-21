namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет вид налога
/// </summary>
public partial class Налог
{
    /// <summary>
    /// Вид налога. Например, НДС
    /// </summary>
    public required string Наименование { get; set; }

    /// <remarks/>
    public bool УчтеноВСумме { get; set; }

    /// <summary>
    /// Флаг, указывающий, что налог является акцизом
    /// </summary>
    public bool Акциз { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.