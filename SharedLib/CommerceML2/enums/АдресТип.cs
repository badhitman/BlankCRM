using System.ComponentModel;

namespace SharedLib.CommerceML2;

/// <remarks/>
public enum АдресТип
{
    /// <remarks/>
    [Description("Почтовый индекс")]
    ПочтовыйИндекс,

    /// <remarks/>
    Страна,

    /// <remarks/>
    Регион,

    /// <remarks/>
    Район,

    /// <remarks/>
    [Description("Населенный пункт")]
    НаселенныйПункт,

    /// <remarks/>
    Город,

    /// <remarks/>
    Улица,

    /// <remarks/>
    Дом,

    /// <remarks/>
    Корпус,

    /// <remarks/>
    Квартира,
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.