////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel;

namespace SharedLib.CommerceML2;

/// <summary>
/// Почтовый адрес
/// </summary>
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