////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/> 
public partial class GLN : ИдентификаторКонтрагента
{
}


/// <summary>
/// Базовый тип идентификаторов контрагента
/// </summary>
[XmlInclude(typeof(ИННРФ))]
[XmlInclude(typeof(GLN))]
[XmlInclude(typeof(КонтрагентИД))]
[XmlRoot("ИдОтправителя", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public abstract partial class ИдентификаторКонтрагента
{
    /// <remarks/>    
    public string Value { get; set; }
}

/// <remarks/>
[XmlInclude(typeof(ИСО3166))]
[XmlInclude(typeof(ОКВЭД))]
[XmlInclude(typeof(ОКП))]
[XmlInclude(typeof(ОКЕИ))]
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
[XmlInclude(typeof(ДокументИД))]
[XmlRoot("НомерДокумента", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
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
[XmlRoot("Стоимость", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class СтоимостьТип
{
    /// <remarks/>
    public СуммаТип Сумма { get; set; }

    /// <remarks/>
    [XmlElement("Налог")]
    public СтоимостьНалогТип[] Налог { get; set; }
}

/// <remarks/>
[XmlRoot("Сумма", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class СуммаТип
{
    /// <remarks/>
    [XmlAttribute(DataType = "integer")]
    public string Валюта { get; set; }

    /// <remarks/>
    [XmlText()]
    public decimal Value { get; set; }
}

/// <remarks/>
[XmlRoot("Налог", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
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
[XmlInclude(typeof(GTIN))]
[XmlInclude(typeof(ТоварИД))]
[XmlRoot("ИдТовара", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
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