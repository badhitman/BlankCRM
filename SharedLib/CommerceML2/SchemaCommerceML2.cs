using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
/// <summary>
/// Собирательный компонент для всего, что может быть упомянуто в процессе обмена
/// </summary>
public partial class КоммерческаяИнформация
{
    /// <remarks/>
    public Классификатор Классификатор { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Документ", typeof(Документ))]
    [System.Xml.Serialization.XmlElementAttribute("ИзмененияПакетаПредложений", typeof(ИзмененияПакетаПредложений))]
    [System.Xml.Serialization.XmlElementAttribute("Каталог", typeof(Каталог))]
    [System.Xml.Serialization.XmlElementAttribute("ПакетПредложений", typeof(ПакетПредложений))]
    public object[] Items { get; set; }

    /// <remarks/>
    public string ВерсияСхемы { get; set; }

    /// <remarks/>
    public DateTime ДатаФормирования { get; set; }
}

/// <summary>
/// Описывает классификацию товаров.
/// </summary>
public partial class Классификатор
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public Контрагент Владелец { get; set; }

    /// <remarks/>
    public string Описание { get; set; }

    /// <remarks/>
    public required Группа[] Группы { get; set; }

    /// <remarks/>
    public required Свойство[] Свойства { get; set; }

    /// <remarks/>
    public required ТипЦены[] ТипыЦен { get; set; }

    /// <remarks/>
    public required Подписант[] Подписанты { get; set; }
}

/// <remarks/>
public partial class Контрагент
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("АдресРегистрации", typeof(Адрес))]
    [System.Xml.Serialization.XmlElementAttribute("ДатаРегистрации", typeof(DateTime), DataType = "date")]
    [System.Xml.Serialization.XmlElementAttribute("ДатаРождения", typeof(DateTime), DataType = "date")]
    [System.Xml.Serialization.XmlElementAttribute("Должность", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ЕГРПО", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ИНН", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Имя", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("КПП", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("МестоРождения", typeof(Адрес))]
    [System.Xml.Serialization.XmlElementAttribute("ОКВЭД", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКДП", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКОПФ", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКПО", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКФС", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Обращение", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Организация", typeof(КонтрагентОрганизация))]
    [System.Xml.Serialization.XmlElementAttribute("ОсновнойВидДеятельности", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Отчество", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОфициальноеНаименование", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Пол", typeof(ПолТип))]
    [System.Xml.Serialization.XmlElementAttribute("ПолноеНаименование", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("РасчетныеСчета", typeof(КонтрагентРасчетныеСчета))]
    [System.Xml.Serialization.XmlElementAttribute("Руководитель", typeof(Руководитель))]
    [System.Xml.Serialization.XmlElementAttribute("УдостоверениеЛичности", typeof(КонтрагентУдостоверениеЛичности))]
    [System.Xml.Serialization.XmlElementAttribute("Фамилия", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ЮридическийАдрес", typeof(Адрес))]
    [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
    public object[] Items { get; set; }

    /// <remarks/>
    public ItemsChoiceType[] ItemsElementName { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }

    /// <remarks/>
    public Адрес Адрес { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформацияКонтакт[] Контакты { get; set; }

    /// <remarks/>
    public required Представитель[] Представители { get; set; }
}

/// <summary>
/// Служит для представления адреса контрагента или любого другого участника бизнес-процесса
/// </summary>
public partial class Адрес
{
    /// <remarks/>
    public string Представление { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("АдресноеПоле")]
    public АдресАдресноеПоле[] АдресноеПоле { get; set; }
}

/// <remarks/>
public partial class АдресАдресноеПоле
{
    /// <remarks/>
    public АдресТип Тип { get; set; }

    /// <remarks/>
    public string Значение { get; set; }
}

/// <summary>
/// Для выгрузки дополнительных документов сопровождающих хоз-операцию и созданных на основании основного документа,
/// например, вместе с заказом покупателя введенные на его основании документы оплаты и отгрузки
/// </summary>
public partial class ПодчиненныйДокумент
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string Номер { get; set; }

    /// <remarks/>
    public DateOnly Дата { get; set; }

    /// <remarks/>
    public ХозОперацияТип ХозОперация { get; set; }

    /// <remarks/>
    public РольТип Роль { get; set; }

    /// <remarks/>

    public bool РольSpecified { get; set; }

    /// <remarks/>
    public string Валюта { get; set; }

    /// <remarks/>
    public string Курс { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>

    public bool СуммаSpecified { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументКонтрагент[] Контрагенты { get; set; }

    /// <remarks/>
    public TimeOnly Время { get; set; }

    /// <remarks/>

    public bool ВремяSpecified { get; set; }

    /// <remarks/>
    public DateTime СрокПлатежа { get; set; }

    /// <remarks/>

    public bool СрокПлатежаSpecified { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументНалог[] Налоги { get; set; }

    /// <remarks/>
    public required Скидка[] Скидки { get; set; }

    /// <remarks/>
    public required ДопРасход[] ДопРасходы { get; set; }

    /// <remarks/>
    public required Склад[] Склады { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументТовар[] Товары { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public required Подписант[] Подписанты { get; set; }
}

/// <remarks/>
public partial class ПодчиненныйДокументКонтрагент : Контрагент
{
    /// <remarks/>
    public РольТип Роль { get; set; }

    /// <remarks/>
    public РасчетныйСчет РасчетныйСчет { get; set; }

    /// <remarks/>
    public Склад Склад { get; set; }
}

/// <summary>
/// Номер расчетного счета контрагента
/// </summary>
public partial class РасчетныйСчет
{
    /// <summary>
    /// Номер счета, сформированный в соответствии со "Схемой обозначения лицевых счетов и их нумерации (по основным счетам)"
    /// приложения 1 Правил ведения бухгалтерского учета в Банке России от 18.09.97 №66
    /// или Правил ведения бухгалтерского учета в кредитных организациях, расположенных на территории РФ, от 05.12.2002 №205
    /// с учетом изменений и дополнений.
    /// </summary>
    [StringLength(20)]
    public string НомерСчета { get; set; }

    /// <remarks/>
    public Банк Банк { get; set; }

    /// <remarks/>
    public Банк БанкКорреспондент { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }
}

/// <summary>
/// Служит для определения реквизитов КО или УБР (БИК, Наименование, Адрес и т.д.) через которые клиент осуществляет расчеты
/// </summary>
public partial class Банк
{
    /// <summary>
    /// Номер счета, сформированный в соответствии со "Схемой обозначения лицевых счетов и их нумерации (по основным счетам)"
    /// приложения 1 Правил ведения бухгалтерского учета в Банке России от 18.09.97 №66
    /// или Правил ведения бухгалтерского учета в кредитных организациях, расположенных на территории РФ, от 05.12.2002 №205
    /// с учетом изменений и дополнений.
    /// </summary>
    [StringLength(20)]
    public string СчетКорреспондентский { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public БанкАдрес Адрес { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформацияКонтакт[] Контакты { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("SWIFT", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("БИК", typeof(string))]
    [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
    public string Item { get; set; }

    /// <remarks/>

    public ItemChoiceType ItemElementName { get; set; }
}

/// <remarks/>
public partial class БанкАдрес : Адрес
{
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:1C.ru:commerceml_2", IncludeInSchema = false)]
public enum ItemChoiceType
{
    /// <remarks/>
    SWIFT,

    /// <remarks/>
    БИК,
}

/// <summary>
/// Наименование и идентификатор склада
/// </summary>
public partial class Склад
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }

    /// <remarks/>
    public Адрес Адрес { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформацияКонтакт[] Контакты { get; set; }
}

/// <remarks/>
public partial class ПодчиненныйДокументНалог : Налог
{
    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public string Ставка { get; set; }
}

/// <summary>
/// Предоставляемая скидка на товарную позицию и/или в целом на сумму документа
/// </summary>
public partial class Скидка
{
    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public string Процент { get; set; }

    /// <remarks/>
    public bool УчтеноВСумме { get; set; }

    /// <remarks/>

    public bool УчтеноВСуммеSpecified { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }
}

/// <summary>
/// Дополнительный расход по номенклатурной позиции и/или по документу в целом (например, транспортировка, тара и т.п.)
/// </summary>
public partial class ДопРасход
{
    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public string Процент { get; set; }

    /// <remarks/>
    public bool УчтеноВСумме { get; set; }

    /// <remarks/>

    public bool УчтеноВСуммеSpecified { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }
}

/// <remarks/>
public partial class ПодчиненныйДокументТовар : Товар
{
    /// <remarks/>
    public string ИдКаталога { get; set; }

    /// <remarks/>
    public string ИдКлассификатора { get; set; }

    /// <remarks/>
    public decimal ЦенаЗаЕдиницу { get; set; }

    /// <remarks/>

    public bool ЦенаЗаЕдиницуSpecified { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>

    public bool КоличествоSpecified { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>

    public bool СуммаSpecified { get; set; }

    /// <remarks/>
    public string Единица { get; set; }

    /// <remarks/>
    public string Коэффициент { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеДанные { get; set; }

    /// <remarks/>
    public string СтранаПроисхождения { get; set; }

    /// <remarks/>
    public string ГТД { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументТоварНалог[] Налоги { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументТоварСкидка[] Скидки { get; set; }

    /// <remarks/>
    public required ДопРасход[] ДопРасходы { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументТоварСклад[] Склады { get; set; }
}

/// <remarks/>
public partial class ПодчиненныйДокументТоварНалог : Налог
{
    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public string Ставка { get; set; }
}

/// <remarks/>
public partial class ПодчиненныйДокументТоварСкидка : Скидка
{
}

/// <remarks/>
public partial class ПодчиненныйДокументТоварСклад : Склад
{
    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>

    public bool КоличествоSpecified { get; set; }
}

/// <summary>
/// Описывает идентифицированный в каталоге товар
/// </summary>
public partial class Товар
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <summary>
    /// Штрихкод (GTIN) товара (код EAN/UPC).
    /// </summary>
    [StringLength(14, MinimumLength = 8)]
    public string? Штрихкод { get; set; }

    /// <remarks/>
    [StringLength(maximumLength: 255)]
    public string? Артикул { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public ТоварБазоваяЕдиница? БазоваяЕдиница { get; set; }

    /// <remarks/>
    public string? ИдТовараУКонтрагента { get; set; }

    /// <remarks/>
    public required string[] Группы { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public string[]? Картинка { get; set; }

    /// <remarks/>
    public string? Страна { get; set; }

    /// <remarks/>
    public string? ТорговаяМарка { get; set; }

    /// <remarks/>
    public Контрагент? ВладелецТорговойМарки { get; set; }

    /// <remarks/>
    public Контрагент? Изготовитель { get; set; }

    /// <remarks/>
    public required ЗначенияСвойства[] ЗначенияСвойств { get; set; }

    /// <remarks/>
    public required ТоварСтавкаНалога[] СтавкиНалогов { get; set; }

    /// <remarks/>
    public required ТоварАкциз[] Акцизы { get; set; }

    /// <remarks/>
    public required ТоварКомплектующее[] Комплектующие { get; set; }

    /// <remarks/>
    public required ТоварАналог[] Аналоги { get; set; }

    /// <remarks/>
    public required ХарактеристикиТовараХарактеристикаТовара[] ХарактеристикиТовара { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ЗначенияРеквизитов { get; set; }

    /// <remarks/>

    public СтатусТип Статус { get; set; }

    /// <remarks/>

    public bool СтатусSpecified { get; set; }
}

/// <remarks/>
public partial class ТоварБазоваяЕдиница
{
    /// <remarks/>
    [Description("Пересчет")]
    public ТоварБазоваяЕдиницаПересчет[] Пересчет { get; set; }

    /// <remarks/>    
    public string[] Text { get; set; }

    /// <remarks/>

    public string Код { get; set; }

    /// <remarks/>

    public string? НаименованиеПолное { get; set; }

    /// <remarks/>
    [StringLength(3)]
    public string? МеждународноеСокращение { get; set; }

    /// <remarks/>

    public string? НаименованиеКраткое { get; set; }
}

/// <summary>
/// Содержит дополнительную информацию о единице измерения товара
/// </summary>
public partial class ТоварБазоваяЕдиницаПересчет
{
    /// <remarks/>
    public string Единица { get; set; }

    /// <summary>
    /// Коэффициент пересчета количества товара в базовую единицу.
    /// </summary>
    public string Коэффициент { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеДанные { get; set; }
}

/// <summary>
/// Определяет значения свойств номенклатурной позиции в каталоге, пакете предложений, документе
/// </summary>
public partial class ЗначенияСвойства
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public string[] Значение { get; set; }
}

/// <remarks/>
public partial class ТоварСтавкаНалога
{
    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public string Ставка { get; set; }
}

/// <remarks/>
public partial class ТоварАкциз
{
    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public decimal СуммаЗаЕдиницу { get; set; }

    /// <remarks/>
    public string Валюта { get; set; }
}

/// <remarks/>
public partial class ТоварКомплектующее : Товар
{
    /// <remarks/>
    public string ИдКаталога { get; set; }

    /// <remarks/>
    public string ИдКлассификатора { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }
}

/// <remarks/>
public partial class ТоварАналог : Товар
{
    /// <remarks/>
    public string ИдКаталога { get; set; }

    /// <remarks/>
    public string ИдКлассификатора { get; set; }
}

/// <summary>
/// Уточняет характеристики поставляемого товара. Товар с разными характеристиками может иметь разную цену
/// </summary>
public partial class ХарактеристикиТовараХарактеристикаТовара
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public string Значение { get; set; }
}

/// <remarks/>
public partial class Подписант
{
    /// <remarks/>
    public string Фамилия { get; set; }

    /// <remarks/>
    public string Имя { get; set; }

    /// <remarks/>
    public string Отчество { get; set; }

    /// <remarks/>
    public string Обращение { get; set; }

    /// <remarks/>
    public ПодписантУдостоверениеЛичности УдостоверениеЛичности { get; set; }

    /// <remarks/>
    public Адрес АдресРегистрации { get; set; }

    /// <remarks/>
    public ПодписантМестоРаботы МестоРаботы { get; set; }

    /// <remarks/>
    public string Должность { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }
}

/// <remarks/>
public partial class ПодписантУдостоверениеЛичности
{
    /// <remarks/>
    public string ВидДокумента { get; set; }

    /// <remarks/>
    public string Серия { get; set; }

    /// <remarks/>
    public string Номер { get; set; }

    /// <remarks/>
    public DateOnly ДатаВыдачи { get; set; }

    /// <remarks/>
    public string КемВыдан { get; set; }
}

/// <remarks/>
public partial class ПодписантМестоРаботы
{
    /// <remarks/>
    public required string ОфициальноеНаименование { get; set; }

    /// <remarks/>
    public Адрес? ЮридическийАдрес { get; set; }

    /// <remarks/>
    public string? ИНН { get; set; }

    /// <remarks/>
    public string? КПП { get; set; }

    /// <remarks/>
    public string? ОсновнойВидДеятельности { get; set; }

    /// <remarks/>
    public string? ЕГРПО { get; set; }

    /// <remarks/>
    public string? ОКВЭД { get; set; }

    /// <remarks/>
    public string? ОКДП { get; set; }

    /// <remarks/>
    public string? ОКОПФ { get; set; }

    /// <remarks/>
    public string? ОКФС { get; set; }

    /// <remarks/>
    public string? ОКПО { get; set; }

    /// <remarks/>
    public DateOnly ДатаРегистрации { get; set; }

    /// <remarks/>

    public bool ДатаРегистрацииSpecified { get; set; }

    /// <remarks/>
    public Руководитель? Руководитель { get; set; }

    /// <remarks/>
    public required РасчетныйСчет[] РасчетныеСчета { get; set; }
}

/// <remarks/>
public partial class Руководитель
{
    /// <remarks/>
    public required string Фамилия { get; set; }

    /// <remarks/>
    public required string Имя { get; set; }

    /// <remarks/>
    public string? Отчество { get; set; }

    /// <remarks/>
    public РуководительУдостоверениеЛичности? УдостоверениеЛичности { get; set; }

    /// <remarks/>
    public Адрес? АресРегистрации { get; set; }

    /// <remarks/>
    public required string Должность { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформацияКонтакт[] Контакты { get; set; }
}

/// <remarks/>
public partial class РуководительУдостоверениеЛичности
{
    /// <remarks/>
    public required string ВидДокумента { get; set; }

    /// <remarks/>
    public string? Серия { get; set; }

    /// <remarks/>
    public string? Номер { get; set; }

    /// <remarks/>
    public DateOnly ДатаВыдачи { get; set; }

    /// <remarks/>
    public string? КемВыдан { get; set; }
}

/// <summary>
/// Идентификатор склада и количество товаров на этом склате
/// </summary>
public partial class ОстаткиПоСкладам
{
    /// <remarks/>
    public required string ИдСклада { get; set; }

    /// <remarks/>

    public decimal КоличествоНаСкладе { get; set; }
}

/// <summary>
/// Цена по номенклатурной позиции
/// </summary>
public partial class Цена
{
    /// <remarks/>
    public required string Представление { get; set; }

    /// <remarks/>
    public required string ИдТипаЦены { get; set; }

    /// <remarks/>
    public decimal ЦенаЗаЕдиницу { get; set; }

    /// <remarks/>
    public string? Валюта { get; set; }

    /// <remarks/>
    public string? Единица { get; set; }

    /// <remarks/>
    public string? Коэффициент { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеДанные { get; set; }

    /// <remarks/>
    public decimal МинКоличество { get; set; }

    /// <remarks/>
    public bool МинКоличествоSpecified { get; set; }

    /// <remarks/>
    public string? ИдКаталога { get; set; }
}

/// <summary>
/// Описывает цену, идентифицированную в каталоге с указанием кода валюты (если ранее не определена)
/// </summary>
public partial class ТипЦены
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Валюта { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Налог")]
    public ТипЦеныНалог[]? Налог { get; set; }
}

/// <remarks/>
public partial class ТипЦеныНалог : Налог
{
}

/// <summary>
/// Описывает свойство товара и возможные варианты значений этого свойства
/// </summary>
public partial class Свойство
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public ОбязательностьСвойствТип Обязательное { get; set; }

    /// <remarks/>

    public bool ОбязательноеSpecified { get; set; }

    /// <remarks/>
    public bool Множественное { get; set; }

    /// <remarks/>

    public bool МножественноеSpecified { get; set; }

    /// <remarks/>
    public ТипЗначенийТип ТипЗначений { get; set; }

    /// <remarks/>

    public bool ТипЗначенийSpecified { get; set; }

    /// <remarks/>
    public СвойствоВариантыЗначений? ВариантыЗначений { get; set; }

    /// <remarks/>
    public bool ДляТоваров { get; set; }

    /// <remarks/>

    public bool ДляТоваровSpecified { get; set; }

    /// <remarks/>
    public bool ДляПредложений { get; set; }

    /// <remarks/>

    public bool ДляПредложенийSpecified { get; set; }

    /// <remarks/>
    public bool ДляДокументов { get; set; }

    /// <remarks/>

    public bool ДляДокументовSpecified { get; set; }
}

/// <remarks/>
public enum ОбязательностьСвойствТип
{
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Для каталога")]
    Длякаталога,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Для документа")]
    Длядокумента,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Для предложений")]
    Дляпредложений,
}

/// <remarks/>
public partial class СвойствоВариантыЗначений
{
    /// <remarks/>
    public string[]? Значение { get; set; }

    /// <remarks/>
    public СвойствоВариантыЗначенийСправочник[]? Справочник { get; set; }
}

/// <remarks/>
public partial class СвойствоВариантыЗначенийСправочник
{
    /// <remarks/>
    public required string ИдЗначения { get; set; }

    /// <remarks/>
    public required string Значение { get; set; }
}

/// <summary>
/// Описывает группу товаров в каталоге
/// </summary>
public partial class Группа
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public required Свойство[] Свойства { get; set; }

    /// <remarks/>
    public required Группа[] Группы { get; set; }
}

/// <remarks/>
public partial class Представитель
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("АдресРегистрации", typeof(Адрес))]
    [System.Xml.Serialization.XmlElementAttribute("ДатаРегистрации", typeof(DateTime), DataType = "date")]
    [System.Xml.Serialization.XmlElementAttribute("ДатаРождения", typeof(DateTime), DataType = "date")]
    [System.Xml.Serialization.XmlElementAttribute("Должность", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ЕГРПО", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ИНН", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Имя", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("КПП", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("МестоРождения", typeof(Адрес))]
    [System.Xml.Serialization.XmlElementAttribute("ОКВЭД", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКДП", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКОПФ", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКПО", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОКФС", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Обращение", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Организация", typeof(КонтрагентОрганизация))]
    [System.Xml.Serialization.XmlElementAttribute("ОсновнойВидДеятельности", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Отчество", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ОфициальноеНаименование", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("Пол", typeof(ПолТип))]
    [System.Xml.Serialization.XmlElementAttribute("ПолноеНаименование", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("РасчетныеСчета", typeof(КонтрагентРасчетныеСчета))]
    [System.Xml.Serialization.XmlElementAttribute("Руководитель", typeof(Руководитель))]
    [System.Xml.Serialization.XmlElementAttribute("УдостоверениеЛичности", typeof(КонтрагентУдостоверениеЛичности))]
    [System.Xml.Serialization.XmlElementAttribute("Фамилия", typeof(string))]
    [System.Xml.Serialization.XmlElementAttribute("ЮридическийАдрес", typeof(Адрес))]
    [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
    public object[]? Items { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]

    public ItemsChoiceType1[]? ItemsElementName { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }

    /// <remarks/>
    public Адрес? Адрес { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформацияКонтакт[] Контакты { get; set; }

    /// <remarks/>
    public string? Отношение { get; set; }
}

/// <remarks/>
public partial class КонтрагентОрганизация
{
    /// <remarks/>
    public required string ОфициальноеНаименование { get; set; }

    /// <remarks/>
    public Адрес? ЮридическийАдрес { get; set; }

    /// <remarks/>
    public string? ИНН { get; set; }

    /// <remarks/>
    public string? КПП { get; set; }

    /// <remarks/>
    public string? ОсновнойВидДеятельности { get; set; }

    /// <remarks/>
    public string? ЕГРПО { get; set; }

    /// <remarks/>
    public string? ОКВЭД { get; set; }

    /// <remarks/>
    public string? ОКДП { get; set; }

    /// <remarks/>
    public string? ОКОПФ { get; set; }

    /// <remarks/>
    public string? ОКФС { get; set; }

    /// <remarks/>
    public string? ОКПО { get; set; }

    /// <remarks/>
    public DateOnly ДатаРегистрации { get; set; }

    /// <remarks/>

    public bool ДатаРегистрацииSpecified { get; set; }

    /// <remarks/>
    public Руководитель? Руководитель { get; set; }

    /// <remarks/>
    public required РасчетныйСчет[] РасчетныеСчета { get; set; }
}

/// <remarks/>
public partial class КонтрагентРасчетныеСчета
{
    /// <remarks/>
    public required РасчетныйСчет[] РасчетныйСчет { get; set; }
}

/// <remarks/>
public partial class КонтрагентУдостоверениеЛичности
{
    /// <remarks/>
    public required string ВидДокумента { get; set; }

    /// <remarks/>
    public string? Серия { get; set; }

    /// <remarks/>
    public string? Номер { get; set; }

    /// <remarks/>
    public DateTime ДатаВыдачи { get; set; }

    /// <remarks/>
    public string? КемВыдан { get; set; }
}

/// <remarks/>
public enum ItemsChoiceType1
{
    /// <remarks/>
    АдресРегистрации,

    /// <remarks/>
    ДатаРегистрации,

    /// <remarks/>
    ДатаРождения,

    /// <remarks/>
    Должность,

    /// <remarks/>
    ЕГРПО,

    /// <remarks/>
    ИНН,

    /// <remarks/>
    Имя,

    /// <remarks/>
    КПП,

    /// <remarks/>
    МестоРождения,

    /// <remarks/>
    ОКВЭД,

    /// <remarks/>
    ОКДП,

    /// <remarks/>
    ОКОПФ,

    /// <remarks/>
    ОКПО,

    /// <remarks/>
    ОКФС,

    /// <remarks/>
    Обращение,

    /// <remarks/>
    Организация,

    /// <remarks/>
    ОсновнойВидДеятельности,

    /// <remarks/>
    Отчество,

    /// <remarks/>
    ОфициальноеНаименование,

    /// <remarks/>
    Пол,

    /// <remarks/>
    ПолноеНаименование,

    /// <remarks/>
    РасчетныеСчета,

    /// <remarks/>
    Руководитель,

    /// <remarks/>
    УдостоверениеЛичности,

    /// <remarks/>
    Фамилия,

    /// <remarks/>
    ЮридическийАдрес,
}

/// <remarks/>
public enum ItemsChoiceType
{
    /// <remarks/>
    АдресРегистрации,

    /// <remarks/>
    ДатаРегистрации,

    /// <remarks/>
    ДатаРождения,

    /// <remarks/>
    Должность,

    /// <remarks/>
    ЕГРПО,

    /// <remarks/>
    ИНН,

    /// <remarks/>
    Имя,

    /// <remarks/>
    КПП,

    /// <remarks/>
    МестоРождения,

    /// <remarks/>
    ОКВЭД,

    /// <remarks/>
    ОКДП,

    /// <remarks/>
    ОКОПФ,

    /// <remarks/>
    ОКПО,

    /// <remarks/>
    ОКФС,

    /// <remarks/>
    Обращение,

    /// <remarks/>
    Организация,

    /// <remarks/>
    ОсновнойВидДеятельности,

    /// <remarks/>
    Отчество,

    /// <remarks/>
    ОфициальноеНаименование,

    /// <remarks/>
    Пол,

    /// <remarks/>
    ПолноеНаименование,

    /// <remarks/>
    РасчетныеСчета,

    /// <remarks/>
    Руководитель,

    /// <remarks/>
    УдостоверениеЛичности,

    /// <remarks/>
    Фамилия,

    /// <remarks/>
    ЮридическийАдрес,
}

/// <summary>
/// Определяет хоз. операцию и реквизиты документа, а также роль предприятия в хоз операции
/// </summary>
public partial class Документ
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string? Номер { get; set; }

    /// <remarks/>
    public DateTime Дата { get; set; }

    /// <remarks/>
    public ХозОперацияТип ХозОперация { get; set; }

    /// <remarks/>
    public РольТип Роль { get; set; }

    /// <remarks/>
    public string? Валюта { get; set; }

    /// <remarks/>
    public string? Курс { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public required ДокументКонтрагент[] Контрагенты { get; set; }

    /// <remarks/>
    public TimeOnly Время { get; set; }

    /// <remarks/>

    public bool ВремяSpecified { get; set; }

    /// <remarks/>
    public DateTime СрокПлатежа { get; set; }

    /// <remarks/>

    public bool СрокПлатежаSpecified { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }

    /// <remarks/>
    public required ДокументНалог[] Налоги { get; set; }

    /// <remarks/>
    public required Скидка[] Скидки { get; set; }

    /// <remarks/>
    public required ДопРасход[] ДопРасходы { get; set; }

    /// <remarks/>
    public required Склад[] Склады { get; set; }

    /// <remarks/>
    public required ДокументТовар[] Товары { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public required Подписант[] Подписанты { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокумент[] ПодчиненныеДокументы { get; set; }
}

/// <remarks/>
public partial class ДокументКонтрагент : Контрагент
{
    /// <remarks/>
    public РольТип Роль { get; set; }

    /// <remarks/>
    public РасчетныйСчет? РасчетныйСчет { get; set; }

    /// <remarks/>
    public Склад? Склад { get; set; }
}

/// <remarks/>
public partial class ДокументНалог : Налог
{
    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public string? Ставка { get; set; }
}

/// <remarks/>
public partial class ДокументТовар : Товар
{
    /// <remarks/>
    public string? ИдКаталога { get; set; }

    /// <remarks/>
    public string? ИдКлассификатора { get; set; }

    /// <remarks/>
    public decimal ЦенаЗаЕдиницу { get; set; }

    /// <remarks/>

    public bool ЦенаЗаЕдиницуSpecified { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>

    public bool КоличествоSpecified { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>

    public bool СуммаSpecified { get; set; }

    /// <remarks/>
    public string? Единица { get; set; }

    /// <remarks/>
    public string? Коэффициент { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеДанные { get; set; }

    /// <remarks/>
    public string? СтранаПроисхождения { get; set; }

    /// <remarks/>
    public string? ГТД { get; set; }

    /// <remarks/>
    public required ДокументТоварНалог[] Налоги { get; set; }

    /// <remarks/>
    public required ДокументТоварСкидка[] Скидки { get; set; }

    /// <remarks/>
    public required ДопРасход[] ДопРасходы { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public required ДокументТоварСклад[] Склады { get; set; }
}

/// <remarks/>
public partial class ДокументТоварНалог : Налог
{
    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public required string Ставка { get; set; }
}

/// <remarks/>
public partial class ДокументТоварСкидка : Скидка
{
}

/// <remarks/>
public partial class ДокументТоварСклад : Склад
{
    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>

    public bool КоличествоSpecified { get; set; }
}

/// <summary>
/// Изменения публикуемых предложений - для быстрой загрузки на сайт
/// </summary>
public partial class ИзмененияПакетаПредложений
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string? ИдКаталога { get; set; }

    /// <remarks/>
    public required ИзмененияПакетаПредложенийПредложение[] Предложения { get; set; }

    /// <remarks/>
    public bool СодержитТолькоИзменения { get; set; }

    /// <remarks/>
    public bool СодержитТолькоИзмененияSpecified { get; set; }
}

/// <remarks/>
public partial class ИзмененияПакетаПредложенийПредложение
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <summary>
    /// Штрихкод (GTIN) товара (код EAN/UPC).
    /// </summary>
    [StringLength(14, MinimumLength = 8)]
    public string? Штрихкод { get; set; }

    /// <remarks/>
    [StringLength(maximumLength: 255)]
    public string? Артикул { get; set; }

    /// <remarks/>
    public string? ИдХарактеристики { get; set; }

    /// <remarks/>
    public string? КодЕдиницыИзмерения { get; set; }

    /// <remarks/>
    public ОстаткиПоСкладам[]? Склады { get; set; }

    /// <remarks/>
    public required Цена[] Цены { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }
}

/// <summary>
/// Каталог товаров содержит перечень товаров.
/// Может составляться разными предприятиями (например, каталог продукции фирмы «1С»).
/// У каталога всегда определен владелец, а товары могут описываться по классификатору.
/// </summary>
public partial class Каталог
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string? ИдКлассификатора { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public Контрагент? Владелец { get; set; }

    /// <remarks/>
    public required Товар[] Товары { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public required Подписант[] Подписанты { get; set; }

    /// <remarks/>    
    public bool СодержитТолькоИзменения { get; set; }

    /// <remarks/>    
    public bool СодержитТолькоИзмененияSpecified { get; set; }
}

/// <summary>
/// Содержит перечень коммерческих предложений.
/// Пакет предложений составляется по определенному (только одному) каталогу, а предложения в пакете могут быть описаны по классификатору.
/// </summary>
public partial class ПакетПредложений
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <remarks/>
    public string? ИдКаталога { get; set; }

    /// <remarks/>
    public string? ИдКлассификатора { get; set; }

    /// <remarks/>
    public DateTime ДействительноС { get; set; }

    /// <remarks/>
    public bool ДействительноСSpecified { get; set; }

    /// <remarks/>
    public DateTime ДействительноДо { get; set; }

    /// <remarks/>
    public bool ДействительноДоSpecified { get; set; }

    /// <remarks/>
    public string? Описание { get; set; }

    /// <remarks/>
    public Контрагент? Владелец { get; set; }

    /// <remarks/>
    public ПакетПредложенийТипыЦен? ТипыЦен { get; set; }

    /// <remarks/>
    public required Склад[] Склады { get; set; }

    /// <remarks/>
    public required ЗначенияСвойства[] ЗначенияСвойств { get; set; }

    /// <remarks/>
    public required ПакетПредложенийПредложение[] Предложения { get; set; }

    /// <remarks/>
    public required Подписант[] Подписанты { get; set; }

    /// <remarks/>
    public bool СодержитТолькоИзменения { get; set; }

    /// <remarks/>
    public bool СодержитТолькоИзмененияSpecified { get; set; }
}

/// <remarks/>
public partial class ПакетПредложенийТипыЦен
{
    /// <remarks/>
    public required string ИдКлассификатора { get; set; }

    /// <remarks/>
    public ТипЦены[]? ТипЦены { get; set; }
}

/// <remarks/>
public partial class ПакетПредложенийПредложение : Товар
{
    /// <remarks/>
    public required Цена[] Цены { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public bool КоличествоSpecified { get; set; }

    /// <remarks/>
    public ОстаткиПоСкладам[]? Склад { get; set; }
}
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.