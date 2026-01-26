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
public abstract partial class КоммерческийДокументИнвойс : КоммерческийДокумент
{

}

/// <remarks/>
public partial class СчетФактура : КоммерческийДокументИнвойс
{
    /// <remarks/>
    public string НомерСчетФактураПоставщик { get; set; }

    /// <remarks/>
    public DateTime ДатаСчетФактураПоставщик { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public string ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    [XmlElement("СтрокаСчетФактура")]
    public СтрокаСчетФактура[] СтрокаСчетФактура { get; set; }
}

/// <remarks/>
public partial class СтрокаСчетФактура
{
    /// <remarks/>
    public string ИдНакладной { get; set; }

    /// <remarks/>
    [XmlElement("ИдТовара")]
    public ИдентификаторТовара[] ИдТовара { get; set; }

    /// <summary>
    /// Представление Кода по Общероссийскому классификатору единиц измерения ОКЕИ
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{3}
    /// </remarks>
    public string? ОКЕИ { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public СтоимостьТип Стоимость { get; set; }
}

/// <remarks/>
public partial class АкцептСчетФактура : КоммерческийДокументИнвойс
{
    /// <remarks/>
    public string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public string НомерСчетФактураКлиент { get; set; }
}

/// <remarks/>
public partial class РеджектСчетФактура : КоммерческийДокументИнвойс
{
    /// <remarks/>
    public string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public string НомерСчетФактураКлиент { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.