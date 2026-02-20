////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceML2;

/// <summary>
/// Описывает цены, которые могут быть использованы при формировании пакета коммерческих предложений
/// </summary>
public partial class ПакетПредложенийТипыЦен
{
    /// <remarks/>
    public required string ИдКлассификатора { get; set; }

    /// <remarks/>
    public ТипЦены[]? ТипЦены { get; set; }
}