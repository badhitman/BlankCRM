////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/> 
public partial class GLN : ИдентификаторКонтрагента
{
}


/// <summary>
/// Базовый тип идентификаторов контрагента
/// </summary>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ИННРФ))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GLN))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(КонтрагентИД))]
[System.Xml.Serialization.XmlRootAttribute("ИдОтправителя", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public abstract partial class ИдентификаторКонтрагента
{
    /// <remarks/>    
    public string Value { get; set; }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ИСО3166))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ОКВЭД))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ОКП))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ОКЕИ))]
public abstract partial class КлассификаторТип
{
    /// <remarks/>    
    public string Value { get; set; }
}

/// <remarks/>
public partial class ИННРФ : ИдентификаторКонтрагента
{
}

/// <remarks/>
public partial class КонтрагентИД : ИдентификаторКонтрагента
{
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ДокументИД))]
[System.Xml.Serialization.XmlRootAttribute("НомерДокумента", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public abstract partial class ИдентификаторДокумента
{
    /// <remarks/>    
    public string Value { get; set; }
}

/// <remarks/>
public partial class ДокументИД : ИдентификаторДокумента
{
}

/// <remarks/>
[System.Xml.Serialization.XmlRootAttribute("Стоимость", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class СтоимостьТип
{
    /// <remarks/>
    public СуммаТип Сумма { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Налог")]
    public СтоимостьНалогТип[] Налог { get; set; }
}

/// <remarks/>
[System.Xml.Serialization.XmlRootAttribute("Сумма", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class СуммаТип
{
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
    public string Валюта { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlTextAttribute()]
    public decimal Value { get; set; }
}

/// <remarks/>
[System.Xml.Serialization.XmlRootAttribute("Налог", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class СтоимостьНалогТип
{
    /// <remarks/>
    public ТипНалога ТипНалога { get; set; }

    /// <remarks/>
    public decimal ВеличинаСтавкиНалога { get; set; }

    /// <remarks/>
    public СуммаТип Сумма { get; set; }

    /// <remarks/>
    public bool ВключеноВСтоимость { get; set; }
}

/// <remarks/>
public enum ТипНалога
{
    /// <remarks/>
    НДС,
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(GTIN))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ТоварИД))]
[System.Xml.Serialization.XmlRootAttribute("ИдТовара", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public abstract partial class ИдентификаторТовара
{
    /// <remarks/>    
    public string Value { get; set; }
}

/// <remarks/>
public partial class GTIN : ИдентификаторТовара
{
}

/// <remarks/>
public partial class ТоварИД : ИдентификаторТовара
{
}

/// <remarks/>
public partial class ОКЕИ : КлассификаторТип
{
}

/// <remarks/>
public partial class ОКП : КлассификаторТип
{
}

/// <remarks/>
public partial class ОКВЭД : КлассификаторТип
{
}

/// <remarks/>
public partial class ИСО3166 : КлассификаторТип
{
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.