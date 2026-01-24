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

/// <summary>
/// Универсальное описание контрагента-участника бизнес-процессов.
/// Содержит описание реквизитов юридического или физического лица контрагента.
/// </summary>
public partial class Контрагент
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
    public required КонтактнаяИнформация[] Контакты { get; set; }

    /// <remarks/>
    public required Представитель[] Представители { get; set; }

    /// <remarks/>
    public РеквизитыФизЛица? РеквизитыФизЛица { get; set; }

    /// <remarks/>
    public РеквизитыЮрЛица? РеквизитыЮрЛица { get; set; }
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
    public РольТип? Роль { get; set; }

    /// <remarks/>
    public string Валюта { get; set; }

    /// <remarks/>
    public string Курс { get; set; }

    /// <summary>
    /// Общая сумма по документу.
    /// Налоги, скидки и дополнительные расходы включаются в данную сумму в зависимости от установленных флажков "УчтеноВСумме"
    /// </summary>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументКонтрагент[] Контрагенты { get; set; }

    /// <remarks/>
    public TimeOnly? Время { get; set; }

    /// <remarks/>
    public DateOnly? СрокПлатежа { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }

    /// <remarks/>
    public required СтавкаСуммаНалога[] Налоги { get; set; }

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
    public required СтавкаСуммаНалога[] Налоги { get; set; }

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
    public string? Штрихкод { get; set; }

    /// <remarks/>
    [StringLength(maximumLength: 255)]
    public string? Артикул { get; set; }

    /// <remarks/>
    public required string Наименование { get; set; }

    /// <summary>
    /// Имя базовой единицы измерения товара по ОКЕИ.
    /// В документах и коммерческих предложениях может быть указана другая единица измерения,
    /// но при этом обязательно указывается коэффициент пересчета количества в базовую единицу товара.
    /// </summary>
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
    public Производитель Производитель { get; set; }

    /// <remarks/>
    public required ЗначенияСвойства[] ЗначенияСвойств { get; set; }

    /// <remarks/>
    public required ТоварСтавкаНалога[] СтавкиНалогов { get; set; }

    /// <remarks/>
    public required ТоварАкциз[] Акцизы { get; set; }

    /// <summary>
    /// Для изделий, содержащих комплектующие
    /// </summary>
    public required ТоварКомплектующее[] Комплектующие { get; set; }

    /// <summary>
    /// Аналоги товара, например для медикаментов другие лекарства, заменяющие данное
    /// </summary>
    public required ТоварАналог[]? Аналоги { get; set; }

    /// <remarks/>
    public required ХарактеристикиТовараХарактеристикаТовара[] ХарактеристикиТовара { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public СтатусТип? Статус { get; set; }
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

    /// <summary>
    /// Например: Г-н, Г-жа, Докт., Проф. и т.д.
    /// </summary>
    public string Обращение { get; set; }

    /// <remarks/>
    public УдостоверениеЛичности УдостоверениеЛичности { get; set; }

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

/// <summary>
/// Цена по номенклатурной позиции
/// </summary>
public partial class Цена
{
    /// <summary>
    /// Представление цены так, как оно отбражается в прайс-листе. Например: 10у.е./за 1000 шт
    /// </summary>
    public required string Представление { get; set; }

    /// <remarks/>
    public required string ИдТипаЦены { get; set; }

    /// <remarks/>
    public decimal ЦенаЗаЕдиницу { get; set; }

    /// <summary>
    /// Код валюты по международному классификатору валют (ISO 4217).
    /// Если не указана, то используется валюта установленная для данного типа цен
    /// </summary>
    [StringLength(3)]
    public required string Валюта { get; set; }

    /// <remarks/>
    public string? Единица { get; set; }

    /// <remarks/>
    public string? Коэффициент { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеДанные { get; set; }

    /// <summary>
    /// Минимальное количество товара в указанных единицах, для которого действует данная цена.
    /// </summary>
    public decimal МинКоличество { get; set; }

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
    public string? Комментарий { get; set; }

    /// <remarks/>
    public Адрес? Адрес { get; set; }

    /// <remarks/>
    public required КонтактнаяИнформация[] Контакты { get; set; }

    /// <remarks/>
    public string? Отношение { get; set; }

    /// <remarks/>
    public РеквизитыФизЛица? РеквизитыФизЛица { get; set; }

    /// <remarks/>
    public РеквизитыЮрЛица? РеквизитыЮрЛица { get; set; }
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
    public required СтавкаСуммаНалога[] Налоги { get; set; }

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
    public required Скидка[] Скидки { get; set; }

    /// <remarks/>
    public required ДопРасход[] ДопРасходы { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеЗначенияРеквизитов { get; set; }

    /// <summary>
    /// Склад, на котором доступен товар и остатки товара на складе
    /// </summary>
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