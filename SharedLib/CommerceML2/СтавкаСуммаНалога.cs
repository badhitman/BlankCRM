////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Вид, ставка и сумма налога.
/// </summary>
public partial class СтавкаСуммаНалога : Налог
{
    /// <remarks/>
    public required decimal Сумма { get; set; }

    /// <remarks/>
    public required string Ставка { get; set; }
}