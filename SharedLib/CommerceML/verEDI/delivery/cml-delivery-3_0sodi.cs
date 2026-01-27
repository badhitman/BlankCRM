////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;
using System.Xml;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
public partial class ТоварАкцептРеджектНакладная
{
    /// <remarks/>
    public string ИдТовара { get; set; }

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
    public string ИдТовара { get; set; }

    /// <remarks/>
    public string НомерАкцептованногоЗаказа { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    [XmlElement("УчетныйНомерСертификата", DataType = "normalizedString")]
    public string[] УчетныйНомерСертификата { get; set; }

    /// <remarks/>
    public СтоимостьТип СуммаПоСтроке { get; set; }

    /// <remarks/>
    public DateTime? СрокРеализации { get; set; }

    /// <remarks/>
    [XmlElement("НомерГТД", DataType = "normalizedString")]
    public string[] НомерГТД { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }
}

/// <remarks/>
public partial class ЭлектроннаяНакладная : КоммерческийДокумент
{
    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдСклада { get; set; }

    /// <remarks/>
    public string АдресСклада { get; set; }

    /// <remarks/>
    public string НомерТоварнойНакладной { get; set; }

    /// <remarks/>
    public DateTime ДатаТоварнойНакладной { get; set; }

    /// <remarks/>
    [XmlElement("Товар")]
    public ТоварВНакладной[] Товар { get; set; }
}

/// <remarks/>
public partial class АкцептНакладной : КоммерческийДокумент
{
    /// <remarks/>
    public string НомерИсходногоДокумента { get; set; }

    /// <remarks/>    
    public string НомерНакладнойКлиента { get; set; }

    /// <remarks/>
    public DateTime ДатаНакладной { get; set; }

    /// <remarks/>
    public DateTime? НачалоРазгрузки { get; set; }

    /// <remarks/>
    public DateTime? ОкончаниеРазгрузки { get; set; }

    /// <remarks/>
    [XmlElement("Товар")]
    public ТоварАкцептРеджектНакладная[] Товар { get; set; }
}

/// <remarks/>
public partial class РеджектНакладной : КоммерческийДокумент
{
    /// <remarks/>
    public string НомерИсходногоДокумента { get; set; }
}

/// <remarks/>
public partial class ЗапросПодтверждениеНакладной : КоммерческийДокумент
{
    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдСклада { get; set; }

    /// <remarks/>
    public string АдресСклада { get; set; }

    /// <remarks/>
    [XmlElement("Товар")]
    public ТоварВНакладной[] Товар { get; set; }
}

/// <remarks/>
public partial class АкцептПодтверждениеНакладной : КоммерческийДокумент
{
    /// <remarks/>
    public string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public DateTime ДатаНакладной { get; set; }
}

/// <remarks/>
public partial class РеджектПодтверждениеНакладной : КоммерческийДокумент
{
    /// <remarks/>
    public string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    [XmlElement("Товар")]
    public ТоварАкцептРеджектНакладная[] Товар { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.