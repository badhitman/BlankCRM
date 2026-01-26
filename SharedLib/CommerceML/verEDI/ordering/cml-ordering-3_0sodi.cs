////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(РеджектЗаказа))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(АкцептЗаказа))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(Заказ))]
public abstract partial class КоммерческийДокументЗаказ
{
    /// <remarks/>
    public ИдентификаторКонтрагента ИдОтправителя { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдПолучателя { get; set; }

    /// <remarks/>
    public ИдентификаторДокумента НомерДокумента { get; set; }

    /// <remarks/>
    public System.DateTime МоментСоздания { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlAnyAttributeAttribute()]
    public System.Xml.XmlAttribute[] AnyAttr { get; set; }
}

/// <remarks/>
public partial class Заказ : КоммерческийДокумент
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдСклада { get; set; }

    /// <remarks/>
    public string АдресСклада { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдПолучателяТовара { get; set; }

    /// <remarks/>
    public System.DateTime ДатаВремяДоставки { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияДоставки { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public СтрокаЗаказа[] Товар { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
[System.Xml.Serialization.XmlRootAttribute("Товар", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class СтрокаЗаказа
{
    /// <remarks/>
    public ИдентификаторТовара ИдТовара { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public СтоимостьТип Стоимость { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class АкцептЗаказа : КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class РеджектЗаказа : КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияРеакции { get; set; }

    /// <remarks/>
    public System.DateTime ДатаВремяДоставки { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияДоставки { get; set; }

    /// <remarks/>
    public СуммаТип ПревышениеЛимита { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public СтрокаЗаказа[] Товар { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.