////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет информацию о товарной позиции (строке документа) в объеме, необходимом для оформления (и передачи) документов.
/// </summary>
public partial class ДокументТовар : Товар
{
    /// <summary>
    /// Идентификатор каталога товаров
    /// </summary>
    public string? ИдКаталога { get; set; }

    /// <summary>
    /// Идентификатор классификатора, в соответствии с которым описан товар в документе
    /// </summary>
    public string? ИдКлассификатора { get; set; }

    /// <summary>
    /// Цена за единицу товара
    /// </summary>
    public decimal ЦенаЗаЕдиницу { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <summary>
    /// Наименование страны (по ОКСМ), из которой ввезен импортный товар согласно таможенным документам
    /// </summary>
    public string? СтранаПроисхождения { get; set; }

    /// <summary>
    /// Номер грузовой таможенной декларации
    /// </summary>
    public string? ГТД { get; set; }

    /// <remarks/>
    public ЕдиницаИзмерения? ЕдиницаИзмерения { get; set; }

    /// <remarks/>
    public СтавкаСуммаНалога[]? Налоги { get; set; }

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