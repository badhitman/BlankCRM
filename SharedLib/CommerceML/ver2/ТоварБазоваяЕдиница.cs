////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Имя базовой единицы измерения товара по ОКЕИ.
/// В документах и коммерческих предложениях может быть указана другая единица измерения,
/// но при этом обязательно указывается коэффициент пересчета количества в базовую единицу товара.
/// </summary>
public partial class ТоварБазоваяЕдиница
{
    /// <summary>
    /// Могут быть указаны способы пересчета в другие единицы.
    /// Указанные способы пересчета следует использовать в случаях несовпадения базовых единиц на одни и те же товары.
    /// </summary>
    public ТоварБазоваяЕдиницаПересчет[]? Пересчет { get; set; }

    /// <summary>
    /// Единица измерения по ОКЕИ
    /// </summary>
    [StringLength(4, MinimumLength = 3)]
    public required string Код { get; set; }

    /// <remarks/>
    [StringLength(255)]
    public string? НаименованиеКраткое { get; set; }

    /// <remarks/>
    [StringLength(255)]
    public string? НаименованиеПолное { get; set; }

    /// <remarks/>
    [StringLength(3)]
    public string? МеждународноеСокращение { get; set; }
}