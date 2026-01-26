////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Для выгрузки дополнительных документов сопровождающих хоз-операцию и созданных на основании основного документа,
/// например, вместе с заказом покупателя введенные на его основании документы оплаты и отгрузки
/// </summary>
public partial class ПодчиненныйДокумент
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <remarks/>
    public required string Номер { get; set; }

    /// <remarks/>
    public required DateOnly Дата { get; set; }

    /// <remarks/>
    public required ХозОперацияТип ХозОперация { get; set; }

    /// <remarks/>
    public РольТип? Роль { get; set; }

    /// <remarks/>
    public required string Валюта { get; set; }

    /// <remarks/>
    public required string Курс { get; set; }

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
    public string? Комментарий { get; set; }

    /// <remarks/>
    public required СтавкаСуммаНалога[] Налоги { get; set; }

    /// <remarks/>
    public Скидка[]? Скидки { get; set; }

    /// <remarks/>
    public ДопРасход[]? ДопРасходы { get; set; }

    /// <remarks/>
    public Склад[]? Склады { get; set; }

    /// <remarks/>
    public required ПодчиненныйДокументТовар[] Товары { get; set; }

    /// <remarks/>
    public ЗначениеРеквизита[]? ЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public Подписант[]? Подписанты { get; set; }
}