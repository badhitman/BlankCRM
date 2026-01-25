////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Цена по номенклатурной позиции
/// </summary>
public partial class Цена
{
    /// <summary>
    /// Представление цены так, как оно отображается в прайс-листе. Например: 10у.е./за 1000 шт
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
    public required ЕдиницаИзмерения ЕдиницаИзмерения { get; set; }

    /// <summary>
    /// Минимальное количество товара в указанных единицах, для которого действует данная цена.
    /// </summary>
    public decimal МинКоличество { get; set; }

    /// <remarks/>
    public string? ИдКаталога { get; set; }
}