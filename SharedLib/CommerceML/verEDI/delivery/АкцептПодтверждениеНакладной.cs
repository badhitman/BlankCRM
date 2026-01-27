////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class АкцептПодтверждениеНакладной : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public DateTime ДатаНакладной { get; set; }
}