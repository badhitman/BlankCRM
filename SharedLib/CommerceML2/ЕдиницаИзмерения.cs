////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет единицу измерения товара и коэффициент пересчета количества в базовую единицу.
/// Если отсутствует, то используется базовая единица товара.
/// </summary>
public partial class ЕдиницаИзмерения
{
    /// <summary>
    /// Имя единицы измерения товара по ОКЕИ.
    /// </summary>
    [StringLength(4, MinimumLength = 3)]
    public required string Единица { get; set; }

    /// <summary>
    /// Коэффициент пересчета количества товара в базовую единицу.
    /// </summary>
    public required string Коэффициент { get; set; }

    /// <remarks/>
    public required ЗначениеРеквизита[] ДополнительныеДанные { get; set; }
}