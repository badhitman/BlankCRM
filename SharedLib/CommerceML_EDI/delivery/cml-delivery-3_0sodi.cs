////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
public partial class ТоварАкцептРеджектНакладная
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ИдТовара")]
    public ИдентификаторТовара[] ИдТовара { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public СтоимостьТип СуммаПоСтроке { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }
}

/// <remarks/>
public partial class ТоварВНакладной
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ИдТовара")]
    public ИдентификаторТовара[] ИдТовара { get; set; }

    /// <remarks/>
    public ИдентификаторДокумента НомерАкцептованногоЗаказа { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("УчетныйНомерСертификата", DataType = "normalizedString")]
    public string[] УчетныйНомерСертификата { get; set; }

    /// <remarks/>
    public СтоимостьТип СуммаПоСтроке { get; set; }

    /// <remarks/>
    public System.DateTime СрокРеализации { get; set; }

    /// <remarks/>
    
    public bool СрокРеализацииSpecified { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("НомерГТД", DataType = "normalizedString")]
    public string[] НомерГТД { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }
}

/// <remarks/>
[System.Xml.Serialization.XmlIncludeAttribute(typeof(РеджектПодтверждениеНакладной))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(АкцептПодтверждениеНакладной))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ЗапросПодтверждениеНакладной))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(РеджектНакладной))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(АкцептНакладной))]
[System.Xml.Serialization.XmlIncludeAttribute(typeof(ЭлектроннаяНакладная))]
public abstract partial class КоммерческийДокументДоставка
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
public partial class ЭлектроннаяНакладная : КоммерческийДокументДоставка
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдСклада { get; set; }

    /// <remarks/>
    public string АдресСклада { get; set; }

    /// <remarks/>
    
    public string НомерТоварнойНакладной { get; set; }

    /// <remarks/>
    public System.DateTime ДатаТоварнойНакладной { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public ТоварВНакладной[] Товар { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class АкцептНакладной : КоммерческийДокументДоставка
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>    
    public string НомерНакладнойКлиента { get; set; }

    /// <remarks/>
    public System.DateTime ДатаНакладной { get; set; }

    /// <remarks/>
    public System.DateTime НачалоРазгрузки { get; set; }

    /// <remarks/>
    
    public bool НачалоРазгрузкиSpecified { get; set; }

    /// <remarks/>
    public System.DateTime ОкончаниеРазгрузки { get; set; }

    /// <remarks/>
    
    public bool ОкончаниеРазгрузкиSpecified { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public ТоварАкцептРеджектНакладная[] Товар { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class РеджектНакладной : КоммерческийДокументДоставка
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class ЗапросПодтверждениеНакладной : КоммерческийДокументДоставка
{
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдСклада { get; set; }

    /// <remarks/>
    public string АдресСклада { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public ТоварВНакладной[] Товар { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class АкцептПодтверждениеНакладной : КоммерческийДокументДоставка
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public System.DateTime ДатаНакладной { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class РеджектПодтверждениеНакладной : КоммерческийДокументДоставка
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Товар")]
    public ТоварАкцептРеджектНакладная[] Товар { get; set; }

    /// <remarks/>
    
    public System.Xml.XmlElement[] Any { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.