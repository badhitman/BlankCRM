////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <remarks/>
public partial class ТоварСтавкаНалога
{
    /// <summary>
    /// Вид налога. Например, НДС
    /// </summary>
    public required string Наименование { get; set; }

    /// <summary>
    /// Ставка налога в процентах
    /// </summary>
    public required string Ставка { get; set; }
}