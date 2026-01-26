////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
[XmlInclude(typeof(ТоварКРаботе))]
[XmlInclude(typeof(КаталогТоваров))]
[XmlInclude(typeof(ЗапросКаталога))]
public abstract partial class КоммерческийДокументКаталог
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
}

/// <remarks/>
public partial class СтрокаТовараКРаботе
{
    /// <remarks/>
    public ИдентификаторТовара ИдТовараКлиента { get; set; }

    /// <remarks/>
    public ИдентификаторТовара[] ИдТовараПоставщика { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public decimal КоэффициентПересчета { get; set; }

    /// <remarks/>
    public bool КоэффициентПересчетаSpecified { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }
}

/// <remarks/>
public partial class СтрокаКаталога
{
    /// <remarks/>
    public ИдентификаторТовара[] ИдТовараПоставщика { get; set; }

    /// <remarks/>
    public ИдентификаторТовара[] ИдТовараКлиента { get; set; }

    /// <remarks/>
    public ИдентификаторТовара[] ШтриховойКод { get; set; }

    /// <remarks/>
    public ОКЕИ ОКЕИ { get; set; }

    /// <remarks/>
    public ОКП ОКП { get; set; }

    /// <summary>
    /// Представление числового кода страны в  соответствии классификатором [МК Стран мира].
    /// 3-значный числовой код страны в  соответствии классификатором [МК Стран мира].
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{3}
    /// </remarks>
    public string? ИСО3166 { get; set; }

    /// <remarks/>
    public ОКВЭД ОКВЭД { get; set; }

    /// <remarks/>
    public КлассификаторТип[] КлассификаторТовара { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public string ТорговаяМарка { get; set; }

    /// <remarks/>
    public string Производитель { get; set; }

    /// <remarks/>
    public string Описание { get; set; }

    /// <remarks/>
    public decimal ВесНетто { get; set; }

    /// <remarks/>
    public bool ВесНеттоSpecified { get; set; }

    /// <remarks/>
    public decimal ВесБрутто { get; set; }

    /// <remarks/>
    public bool ВесБруттоSpecified { get; set; }

    /// <remarks/>
    public decimal ВысотаСлояТовара { get; set; }

    /// <remarks/>
    public bool ВысотаСлояТовараSpecified { get; set; }

    /// <remarks/>
    public decimal ВысотаТовара { get; set; }

    /// <remarks/>
    public bool ВысотаТовараSpecified { get; set; }

    /// <remarks/>
    public decimal ШиринаТовара { get; set; }

    /// <remarks/>
    public bool ШиринаТовараSpecified { get; set; }

    /// <remarks/>
    public decimal ГлубинаТовара { get; set; }

    /// <remarks/>
    public bool ГлубинаТовараSpecified { get; set; }

    /// <remarks/>
    public decimal ОбъемТовара { get; set; }

    /// <remarks/>
    public bool ОбъемТовараSpecified { get; set; }

    /// <remarks/>
    public decimal МинКоличествоДляЗаказа { get; set; }

    /// <remarks/>
    public bool МинКоличествоДляЗаказаSpecified { get; set; }

    /// <remarks/>
    public decimal КоличествоВСлоеНаЕвропалете { get; set; }

    /// <remarks/>
    public bool КоличествоВСлоеНаЕвропалетеSpecified { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "duration")]
    public TimeSpan СрокХранения { get; set; }

    /// <remarks/>
    [XmlElement(DataType = "integer")]
    public int ТемператураХранения { get; set; }

    /// <remarks/>
    public decimal Кратность { get; set; }

    /// <remarks/>
    public bool КратностьSpecified { get; set; }

    /// <remarks/>
    public decimal КоличесвоЕдиницОбъектаВерхнегоУровня { get; set; }

    /// <remarks/>
    public bool КоличесвоЕдиницОбъектаВерхнегоУровняSpecified { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }

    /// <remarks/>
    [XmlAnyElement()]
    public System.Xml.XmlElement[] Any { get; set; }
}

/// <remarks/>
public partial class ЗапросКаталога : КоммерческийДокументКаталог
{
    /// <remarks/>
    public TimeSpan ДлительностьОжиданияОтвета { get; set; }
}

/// <remarks/>
public partial class КаталогТоваров : КоммерческийДокументКаталог
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public TimeSpan ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public bool ЭтоПолныйКаталог { get; set; }

    /// <remarks/>

    public bool ЭтоПолныйКаталогSpecified { get; set; }

    /// <remarks/>
    public СтрокаКаталога[] Товар { get; set; }
}

/// <remarks/>
public partial class ТоварКРаботе : КоммерческийДокументКаталог
{
    /// <remarks/>
    public ИдентификаторДокумента НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public СтрокаТовараКРаботе[] Товар { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.