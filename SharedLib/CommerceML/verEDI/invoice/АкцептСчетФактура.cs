////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class АкцептСчетФактура : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public required string НомерСчетФактураКлиент { get; set; }
}