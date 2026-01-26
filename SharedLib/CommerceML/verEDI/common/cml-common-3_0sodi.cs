////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Xml.Serialization;

namespace SharedLib.CommerceMLEDI;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <remarks/>
public abstract partial class КоммерческийДокумент
{
    /// <remarks/>
    public ИдентификаторКонтрагента ИдОтправителя { get; set; }

    /// <remarks/>
    public ИдентификаторКонтрагента ИдПолучателя { get; set; }

    /// <remarks/>
    public ИдентификаторДокумента НомерДокумента { get; set; }

    /// <remarks/>
    public DateTime МоментСоздания { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }
}

/// <remarks/>
public partial class КлассификаторТип
{
    /// <summary>
    /// Представление числового кода страны в  соответствии классификатором [МК Стран мира]
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{3}
    /// </remarks>
    public string? ИСО3166 { get; set; }

    /// <summary>
    /// Представление Кода по Общероссийскому классификатору внешнеэкономической деятельности ОКВЭД
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{2}.[0-9]{2}.[0-9]{2}
    /// </remarks>
    public string? ОКВЭД { get; set; }

    /// <summary>
    /// Представление Кода по Общероссийскому классификатору продукции ОКП
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{6}
    /// </remarks>
    public string? ОКП { get; set; }

    /// <summary>
    /// Представление Кода по Общероссийскому классификатору единиц измерения ОКЕИ
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{3}
    /// </remarks>
    public string? ОКЕИ { get; set; }
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
public partial class ИдентификаторТовара
{
    /// <summary>
    /// Идентификатор товара (Global Trade Item Number)
    /// </summary>
    public string? GTIN { get; set; }

    /// <summary>
    /// Код товара в справочнике Ассоциации автоматической идентификации GS1 Global Trade Item Number
    /// </summary>
    public string? ТоварИД { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.