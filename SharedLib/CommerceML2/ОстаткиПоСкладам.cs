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
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.