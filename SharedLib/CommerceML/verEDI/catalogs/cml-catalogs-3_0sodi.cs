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
public abstract partial class КоммерческийДокументКаталог : КоммерческийДокумент
{

}

/// <remarks/>
public partial class СтрокаТовараКРаботе
{
    /// <remarks/>
    public string ИдТовараКлиента { get; set; }

    /// <remarks/>
    public string ИдТовараПоставщика { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public decimal? КоэффициентПересчета { get; set; }

    /// <remarks/>
    public string Примечание { get; set; }
}

/// <summary>
/// Информация о единице товара, присутствующей в каталоге
/// </summary>
public partial class СтрокаКаталога
{
    /// <remarks/>
    public string ИдТовараПоставщика { get; set; }

    /// <remarks/>
    public string ИдТовараКлиента { get; set; }

    /// <remarks/>
    public string ШтриховойКод { get; set; }

    /// <summary>
    /// Представление Кода по Общероссийскому классификатору единиц измерения ОКЕИ
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{3}
    /// </remarks>
    public string? ОКЕИ { get; set; }

    /// <summary>
    /// Представление Кода по Общероссийскому классификатору продукции ОКП
    /// </summary>
    /// <remarks>
    /// pattern: [0-9]{6}
    /// </remarks>
    public string? ОКП { get; set; }

    /// <summary>
    /// Представление числового кода страны в  соответствии классификатором [МК Стран мира].
    /// 3-значный числовой код страны в  соответствии классификатором [МК Стран мира].
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

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public string ТорговаяМарка { get; set; }

    /// <remarks/>
    public string Производитель { get; set; }

    /// <remarks/>
    public string Описание { get; set; }

    /// <remarks/>
    public decimal? ВесНетто { get; set; }

    /// <remarks/>
    public decimal? ВесБрутто { get; set; }

    /// <remarks/>
    public decimal? ВысотаСлояТовара { get; set; }

    /// <remarks/>
    public decimal? ВысотаТовара { get; set; }

    /// <remarks/>
    public decimal? ШиринаТовара { get; set; }

    /// <remarks/>
    public decimal? ГлубинаТовара { get; set; }

    /// <remarks/>
    public decimal? ОбъемТовара { get; set; }

    /// <remarks/>
    public decimal? МинКоличествоДляЗаказа { get; set; }

    /// <remarks/>
    public decimal? КоличествоВСлоеНаЕвропалете { get; set; }

    /// <remarks/>
    public TimeSpan? СрокХранения { get; set; }

    /// <remarks/>
    public int? ТемператураХранения { get; set; }

    /// <remarks/>
    public decimal? Кратность { get; set; }

    /// <remarks/>
    public decimal? КоличествоЕдиницОбъектаВерхнегоУровня { get; set; }

    /// <remarks/>
    public string? Примечание { get; set; }
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
    public string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public TimeSpan ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public bool? ЭтоПолныйКаталог { get; set; }

    /// <remarks/>
    public СтрокаКаталога[] Товар { get; set; }
}

/// <summary>
/// Тип документа "Каталог товаров"
/// </summary>
public partial class ТоварКРаботе : КоммерческийДокументКаталог
{
    /// <remarks/>
    public string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public СтрокаТовараКРаботе[] Товар { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.