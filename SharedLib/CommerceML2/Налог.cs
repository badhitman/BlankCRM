////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Определяет вид налога
/// </summary>
public partial class Налог
{
    /// <summary>
    /// Вид налога. Например, НДС
    /// </summary>
    public required string Наименование { get; set; }

    /// <remarks/>
    public bool УчтеноВСумме { get; set; }

    /// <summary>
    /// Флаг, указывающий, что налог является акцизом
    /// </summary>
    public bool Акциз { get; set; }
}