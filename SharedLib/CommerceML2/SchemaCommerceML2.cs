namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class КоммерческаяИнформация1 : КоммерческаяИнформация
{
}

/// <remarks/>
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

/// <remarks/>
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

/// <remarks/>
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

/// <remarks/>
public enum АдресТип
{
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Почтовый индекс")]
    Почтовыйиндекс,

    /// <remarks/>
    Страна,

    /// <remarks/>
    Регион,

    /// <remarks/>
    Район,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Населенный пункт")]
    Населенныйпункт,

    /// <remarks/>
    Город,

    /// <remarks/>
    Улица,

    /// <remarks/>
    Дом,

    /// <remarks/>
    Корпус,

    /// <remarks/>
    Квартира,
}

/// <remarks/>
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
public enum ХозОперацияТип
{
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Заказ товара")]
    Заказтовара,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Счет на оплату")]
    Счетнаоплату,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Отпуск товара")]
    Отпусктовара,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Счет-фактура")]
    Счетфактура,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Возврат товара")]
    Возвраттовара,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Передача товара на комиссию")]
    Передачатоваранакомиссию,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Возврат комиссионного товара")]
    Возвраткомиссионноготовара,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Отчет о продажах комиссионного товара")]
    Отчетопродажахкомиссионноготовара,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Отчет о списании комиссионного товара")]
    Отчетосписаниикомиссионноготовара,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Выплата наличных денег")]
    Выплатаналичныхденег,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Возврат наличных денег")]
    Возвратналичныхденег,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Выплата безналичных денег")]
    Выплатабезналичныхденег,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Возврат безналичных денег")]
    Возвратбезналичныхденег,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Переоценка товаров")]
    Переоценкатоваров,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Передача прав")]
    Передачаправ,

    /// <remarks/>
    Прочие,
}

/// <remarks/>
public enum РольТип
{
    /// <remarks/>
    Продавец,

    /// <remarks/>
    Покупатель,

    /// <remarks/>
    Плательщик,

    /// <remarks/>
    Получатель,

    /// <remarks/>
    Комитент,

    /// <remarks/>
    Комиссионер,

    /// <remarks/>
    Лицензиар,

    /// <remarks/>
    Лицензиат,
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

/// <remarks/>
public partial class РасчетныйСчет
{
    /// <remarks/>
    public string НомерСчета { get; set; }

    /// <remarks/>
    public Банк Банк { get; set; }

    /// <remarks/>
    public Банк БанкКорреспондент { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }
}

/// <remarks/>
public partial class Банк
{
    /// <remarks/>
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
public partial class КонтактнаяИнформацияКонтакт
{
    /// <remarks/>
    public КонтактТип Тип { get; set; }

    /// <remarks/>
    public string Значение { get; set; }

    /// <remarks/>
    public string Комментарий { get; set; }
}

/// <remarks/>
public enum КонтактТип
{
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Телефон внутренний")]
    Телефонвнутренний,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Телефон рабочий")]
    Телефонрабочий,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Телефон мобильный")]
    Телефонмобильный,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Телефон домашний")]
    Телефондомашний,

    /// <remarks/>
    Пейджер,

    /// <remarks/>
    Факс,

    /// <remarks/>
    Почта,

    /// <remarks/>
    ICQ,

    /// <remarks/>
    ВебСайт,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("Координаты на карте")]
    Координатынакарте,

    /// <remarks/>
    Прочее,
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

/// <remarks/>
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

/// <remarks/>
public partial class Налог
{
    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    public bool УчтеноВСумме { get; set; }

    /// <remarks/>

    public bool УчтеноВСуммеSpecified { get; set; }

    /// <remarks/>
    public bool Акциз { get; set; }

    /// <remarks/>

    public bool АкцизSpecified { get; set; }
}

/// <remarks/>
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

/// <remarks/>
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
public partial class ЗначениеРеквизита
{
    /// <remarks/>
    public string Наименование { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Значение")]
    public string[] Значение { get; set; }
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

/// <remarks/>
public partial class Товар
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public string? Штрихкод { get; set; }

    /// <remarks/>
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
    [System.Xml.Serialization.XmlElementAttribute("Пересчет")]
    public ТоварБазоваяЕдиницаПересчет[] Пересчет { get; set; }

    /// <remarks/>    
    public string[] Text { get; set; }

    /// <remarks/>

    public string Код { get; set; }

    /// <remarks/>

    public string НаименованиеПолное { get; set; }

    /// <remarks/>

    public string МеждународноеСокращение { get; set; }

    /// <remarks/>

    public string НаименованиеКраткое { get; set; }
}

/// <remarks/>
public partial class ТоварБазоваяЕдиницаПересчет
{
    /// <remarks/>
    public string Единица { get; set; }

    /// <remarks/>
    public string Коэффициент { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеДанные { get; set; }
}

/// <remarks/>
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

/// <remarks/>
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
public enum СтатусТип
{
    /// <remarks/>
    Новый,

    /// <remarks/>
    Изменен,

    /// <remarks/>
    Удален,
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

/// <remarks/>
public partial class ОстаткиПоСкладам
{
    /// <remarks/>

    public required string ИдСклада { get; set; }

    /// <remarks/>

    public decimal КоличествоНаСкладе { get; set; }

    /// <remarks/>

    public bool КоличествоНаСкладеSpecified { get; set; }
}

/// <remarks/>
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

/// <remarks/>
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

/// <remarks/>
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
public enum ТипЗначенийТип
{
    /// <remarks/>
    Строка,

    /// <remarks/>
    Число,

    /// <remarks/>
    Время,

    /// <remarks/>
    Справочник,
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

/// <remarks/>
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
public enum ПолТип
{
    /// <remarks/>
    М,

    /// <remarks/>
    Ж,
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

/// <remarks/>
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

/// <remarks/>
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

    /// <remarks/>
    public string? Штрихкод { get; set; }

    /// <remarks/>
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

    /// <remarks/>

    public bool КоличествоSpecified { get; set; }
}

/// <remarks/>
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

/// <remarks/>
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