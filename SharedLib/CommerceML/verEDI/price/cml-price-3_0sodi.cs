////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
[XmlInclude(typeof(ПрайсЛистКРаботе))]
[XmlInclude(typeof(ПрайсЛист))]
[XmlInclude(typeof(ЗапросПрайсЛист))]
public abstract partial class КоммерческийДокументПрайс : КоммерческийДокумент
{

}

/// <remarks/>
public partial class ЗапросПрайсЛист : КоммерческийДокументПрайс
{
    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>    
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class ПрайсЛист : КоммерческийДокументПрайс
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "duration")]
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
    [XmlElement("ТоварПрайсЛист")]
    public ТоварПрайсЛист[] ТоварПрайсЛист { get; set; }
}

/// <remarks/>
[XmlRoot("ТоварПрайЛист", Namespace = "urn:moo-sodi.ru:commerceml_sodi", IsNullable = false)]
public partial class ТоварПрайсЛист
{
    /// <remarks/>
    [XmlElement("ИдТовара")]
    public ИдентификаторТовара[] ИдТовара { get; set; }

    /// <remarks/>
    public Цена Цена { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }
}

/// <remarks/>

public partial class Цена : СтоимостьТип
{
}

/// <remarks/>
public partial class ПрайсЛистКРаботе : КоммерческийДокументПрайс
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public DateTime? НачалоДействия { get; set; }

    /// <remarks/>
    public DateTime? ОкончаниеДействия { get; set; }

    /// <remarks/>
    public ТоварПрайсЛист[] ТоварПрайсЛист { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.