////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class ИзмененияПакетаПредложенийПредложение
{
    /// <remarks/>
    public required string Ид { get; set; }

    /// <summary>
    /// Идентификатор характеристики товара
    /// </summary>
    public string? ИдХарактеристики { get; set; }

    /// <remarks/>
    public string? КодЕдиницыИзмерения { get; set; }

    /// <remarks/>
    public ОстаткиПоСкладам[]? Склады { get; set; }

    /// <remarks/>
    public Цена[]? Цены { get; set; }

    /// <summary>
    /// Остаток товара на складах
    /// </summary>
    public decimal Количество { get; set; }
}