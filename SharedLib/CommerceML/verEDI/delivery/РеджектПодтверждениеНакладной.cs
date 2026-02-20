////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class РеджектПодтверждениеНакладной : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public required ТоварАкцептРеджектНакладная[] Товар { get; set; }
}