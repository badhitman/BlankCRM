////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
[XmlInclude(typeof(РеджектЗаказа))]
[XmlInclude(typeof(АкцептЗаказа))]
[XmlInclude(typeof(Заказ))]
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
    [XmlAnyAttribute()]
    public System.Xml.XmlAttribute[] AnyAttr { get; set; }
}

/// <remarks/>
public partial class Заказ : КоммерческийДокументЗаказ
{
    /// <remarks/>
    [XmlElement(DataType = "duration")]
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
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияДоставки { get; set; }

    /// <remarks/>
    [XmlElement("Товар")]
    public СтрокаЗаказа[] Товар { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
[XmlRoot("Товар", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
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
public partial class АкцептЗаказа : КоммерческийДокументЗаказ
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class РеджектЗаказа : КоммерческийДокументЗаказ
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияРеакции { get; set; }

    /// <remarks/>
    public System.DateTime ДатаВремяДоставки { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияДоставки { get; set; }

    /// <remarks/>
    public СуммаТип ПревышениеЛимита { get; set; }

    /// <remarks/>
    [XmlElement("Товар")]
    public СтрокаЗаказа[] Товар { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.