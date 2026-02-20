////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Определяет сумму и код валюты
/// </summary>
public partial class СуммаТип
{
    /// <summary>
    /// 3-значный числовой код валюты в соответствии с классификатором [ОК Валют]
    /// </summary>
    public int Валюта { get; set; }

    /// <remarks/>
    public decimal Value { get; set; }
}