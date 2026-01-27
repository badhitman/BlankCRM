////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib.CommerceMLEDI;

/// <summary>
/// Документ "Прайс-лист" формируется поставщиком и передается клиенту в ответ на запрос на предоставление прайс-листа или в случае произошедших в прайс-листе изменений
/// </summary>
public partial class ПрайсЛист : КоммерческийДокумент
{
    /// <remarks/>
    public required string НомерИсходногоДокумента { get; set; }

    /// <summary>
    /// Период времени с момента создания документа, в течение которого ожидается реакция на данный документ.
    /// </summary>
    public TimeSpan? ДлительностьОжиданияОтвета { get; set; }

    /// <remarks/>
    public bool? ПолныйПрайсЛист { get; set; }

    /// <remarks/>
    public DateTime? НачалоДействия { get; set; }

    /// <remarks/>
    public DateTime? ОкончаниеДействия { get; set; }

    /// <remarks/>
    public required ТоварПрайсЛист[] ТоварПрайсЛист { get; set; }
}