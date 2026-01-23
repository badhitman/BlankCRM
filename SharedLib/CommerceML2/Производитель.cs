namespace SharedLib.CommerceML2;

/// <summary>
/// Содержит описание страны, непосредственного изготовителя и торговой марки товара
/// </summary>
public class Производитель
{
    /// <remarks/>
    public string? Страна { get; set; }

    /// <remarks/>
    public string? ТорговаяМарка { get; set; }

    /// <remarks/>
    public Контрагент? ВладелецТорговойМарки { get; set; }

    /// <remarks/>
    public Контрагент? Изготовитель { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.