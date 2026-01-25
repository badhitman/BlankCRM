////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет информацию о товарной позиции (строке документа) в объеме, необходимом для оформления (и передачи) документов.
/// </summary>
public partial class ПодчиненныйДокументТовар : Товар
{
    /// <remarks/>
    public required string ИдКаталога { get; set; }

    /// <remarks/>
    public required string ИдКлассификатора { get; set; }

    /// <remarks/>
    public decimal ЦенаЗаЕдиницу { get; set; }

    /// <remarks/>
    public decimal Количество { get; set; }

    /// <remarks/>
    public decimal Сумма { get; set; }

    /// <remarks/>
    public ЕдиницаИзмерения? ЕдиницаИзмерения { get; set; }

    /// <remarks/>
    public string? СтранаПроисхождения { get; set; }

    /// <remarks/>
    public string? ГТД { get; set; }

    /// <remarks/>
    public required СтавкаСуммаНалога[] Налоги { get; set; }

    /// <remarks/>
    public Скидка[]? Скидки { get; set; }

    /// <remarks/>
    public ДопРасход[]? ДопРасходы { get; set; }

    /// <remarks/>
    public ЗначениеРеквизита[]? ДополнительныеЗначенияРеквизитов { get; set; }

    /// <remarks/>
    public ПодчиненныйДокументТоварСклад[]? Склады { get; set; }
}