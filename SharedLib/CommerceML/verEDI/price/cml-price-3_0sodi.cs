////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ПрайсЛистКРаботе))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ПрайсЛист))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ЗапросПрайсЛист))]
public abstract partial class КоммерческийДокументПрайс
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
public partial class ЗапросПрайсЛист : КоммерческийДокумент
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class ПрайсЛист : КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public bool ПолныйПрайсЛист { get; set; }

    /// <remarks/>
    
    public bool ПолныйПрайсЛистSpecified { get; set; }

    /// <remarks/>
    public System.DateTime НачалоДействия { get; set; }

    /// <remarks/>
    
    public bool НачалоДействияSpecified { get; set; }

    /// <remarks/>
    public System.DateTime ОкончаниеДействия { get; set; }

    /// <remarks/>
    
    public bool ОкончаниеДействияSpecified { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ТоварПрайсЛист")]
    public ТоварПрайсЛист[] ТоварПрайсЛист { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
[System.Xml.Serialization.XmlRootAttribute("ТоварПрайЛист", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class ТоварПрайсЛист
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ИдТовара")]
    public ИдентификаторТовара[] ИдТовара { get; set; }

    /// <remarks/>
    public Цена Цена { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:moo-sodi.ru:commerceml_sodi")]

public partial class Цена : СтоимостьТип
{
}

/// <remarks/>
public partial class ПрайсЛистКРаботе : КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public System.DateTime НачалоДействия { get; set; }

    /// <remarks/>
    
    public bool НачалоДействияSpecified { get; set; }

    /// <remarks/>
    public System.DateTime ОкончаниеДействия { get; set; }

    /// <remarks/>
    
    public bool ОкончаниеДействияSpecified { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ТоварПрайсЛист")]
    public ТоварПрайсЛист[] ТоварПрайсЛист { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.