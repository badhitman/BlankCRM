////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(РеджектСчетФактура))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(АкцептСчетФактура))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(СчетФактура))]
public abstract partial class КоммерческийДокументИнвойс
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
public partial class СчетФактура : КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторДокумента НомерСчетФактураПоставщик { get; set; }

    /// <remarks/>
    public System.DateTime ДатаСчетФактураПоставщик { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("СтрокаСчетФактура")]
    public СтрокаСчетФактура[] СтрокаСчетФактура { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class СтрокаСчетФактура
{
    /// <remarks/>
    public ИдентификаторДокумента ИдНакладной { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ИдТовара")]
    public ИдентификаторТовара[] ИдТовара { get; set; }

    /// <remarks/>
    public ОКЕИ ОКЕИ { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public СтоимостьТип Стоимость { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class АкцептСчетФактура : КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public ИдентификаторДокумента НомерСчетФактураКлиент { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class РеджектСчетФактура : КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public ИдентификаторДокумента НомерСчетФактураКлиент { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.