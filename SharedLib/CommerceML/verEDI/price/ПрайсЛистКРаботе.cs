////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Документ "Прайс-лист, принятый к работе" формируется клиентом и передается поставщику в момент утверждения ценовых предложений поставщика
/// </summary>
public partial class ПрайсЛистКРаботе : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <remarks/>
    public DateTime? НачалоДействия { get; set; }

    /// <remarks/>
    public DateTime? ОкончаниеДействия { get; set; }

    /// <remarks/>
    public required ТоварПрайсЛист[] ТоварПрайсЛист { get; set; }
}