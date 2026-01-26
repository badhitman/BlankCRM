////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
[XmlInclude(typeof(РеджектСчетФактура))]
[XmlInclude(typeof(АкцептСчетФактура))]
[XmlInclude(typeof(СчетФактура))]
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
    [XmlAnyAttribute()]
    public System.Xml.XmlAttribute[] AnyAttr { get; set; }
}

/// <remarks/>
public partial class СчетФактура : КоммерческийДокументИнвойс
{
    /// <remarks/>
    public ИдентификаторДокумента НомерСчетФактураПоставщик { get; set; }

    /// <remarks/>
    public System.DateTime ДатаСчетФактураПоставщик { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    [XmlElement("СтрокаСчетФактура")]
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
    [XmlElement("ИдТовара")]
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
public partial class АкцептСчетФактура : КоммерческийДокументИнвойс
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public ИдентификаторДокумента НомерСчетФактураКлиент { get; set; }

    /// <remarks/>

    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class РеджектСчетФактура : КоммерческийДокументИнвойс
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public ИдентификаторДокумента НомерСчетФактураКлиент { get; set; }

    /// <remarks/>

    public System.Xml.XmlElement[] Any { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.