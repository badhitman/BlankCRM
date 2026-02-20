////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет хоз. операцию и реквизиты документа, а также роль предприятия в хоз операции
/// </summary>
public partial class Документ
{
    /// <summary>
    /// Идентификатор документа уникальный в рамках файла обмена
    /// </summary>
    public required string Ид { get; set; }

    /// <remarks/>
    public string? Номер { get; set; }

    /// <summary>
    /// Дата документа
    /// </summary>
    public required DateTime Дата { get; set; }

    /// <summary>
    /// Возможные значения:
    ///     ЗаказТовара; CчетНаОплату; ОтпускТовара; СчетФактура;
    ///     ВозвратТовара; ПередачаТовараНаКомиссию; ВозвратКомиссионногоТовара;
    ///     ОтчетОПродажахКомиссионногоТовара; ВыплатаНаличныхДенег;
    ///     ВозвратНаличныхДенег; ВыплатаБезналичныхДенег; ВозвратБезналичныхДенег;
    ///     Переоценка товаров; Передача прав; Прочее.
    /// </summary>
    public required ХозОперацияТип ХозОперация { get; set; }

    /// <summary>
    /// Роль предприятия в документе. Возможные значения:
    ///     Продавец; Покупатель; Плательщик; Получатель;
    ///     Комитент; Комиссионер; Лицензиар; Лицензиант.
    /// </summary>
    public required РольТип Роль { get; set; }

    /// <summary>
    /// Код валюты по международному классификатору валют (ISO 4217)
    /// </summary>
    [StringLength(3)]
    public required string Валюта { get; set; }

    /// <summary>
    /// Курс указанной валюты по отношению к национальной валюте.
    /// </summary>
    /// <remarks>
    /// Для национальной валюты курс равен 1.
    /// </remarks>
    public required string Курс { get; set; }

    /// <summary>
    /// Общая сумма по документу. Налоги, скидки и дополнительные расходы включаются в данную сумму в зависимости от установленных флажков "УчтеноВСумме"
    /// </summary>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public required ДокументКонтрагент[] Контрагенты { get; set; }

    /// <remarks/>
    public TimeOnly? Время { get; set; }

    /// <remarks/>
    public DateTime? СрокПлатежа { get; set; }

    /// <remarks/>
    public string? Комментарий { get; set; }

    /// <remarks/>
    public required СтавкаСуммаНалога[] Налоги { get; set; }

    /// <remarks/>
    public Скидка[]? Скидки { get; set; }

    /// <summary>
    /// Дополнительный расход, сумма, проценты.
    /// </summary>
    public ДопРасход[]? ДопРасходы { get; set; }

    /// <summary>
    /// Склад в документе. на который осуществляется доставка или с которого производится отгрузка
    /// </summary>
    public required Склад[] Склады { get; set; }

    /// <remarks/>
    public required ДокументТовар[] Товары { get; set; }

    /// <remarks/>
    public ЗначениеРеквизита[]? ЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public Подписант[]? Подписанты { get; set; }

    /// <remarks/>
    public ПодчиненныйДокумент[]? ПодчиненныеДокументы { get; set; }
}