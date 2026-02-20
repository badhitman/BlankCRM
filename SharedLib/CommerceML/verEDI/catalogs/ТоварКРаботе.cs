////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Тип документа "Каталог товаров"
/// </summary>
public partial class ТоварКРаботе : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public required СтрокаТовараКРаботе[] Товар { get; set; }
}