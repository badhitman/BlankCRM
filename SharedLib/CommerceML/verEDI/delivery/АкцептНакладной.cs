////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <remarks/>
public partial class АкцептНакладной : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <remarks/>    
    public required string НомерНакладнойКлиента { get; set; }

    /// <remarks/>
    public DateTime ДатаНакладной { get; set; }

    /// <remarks/>
    public DateTime? НачалоРазгрузки { get; set; }

    /// <remarks/>
    public DateTime? ОкончаниеРазгрузки { get; set; }

    /// <remarks/>
    public required ТоварАкцептРеджектНакладная[] Товар { get; set; }
}