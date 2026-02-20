////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ICommerceTransmission
/// </summary>
public partial interface ICommerceTransmission : ICommerceServiceBase
{/// <summary>
 /// Смена статуса заказу по идентификатору HelpDesk документа
 /// </summary>
 /// <remarks>
 /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
 /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
 /// </remarks>
    public Task<TResponseModel<OrderDocumentModelDB[]>> StatusOrderChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Смена статуса заявки (бронь)
    /// </summary>
    public Task<TResponseModel<List<RecordsAttendanceModelDB>>> StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// WorkSchedulesFind
    /// </summary>
    public Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, CancellationToken token = default);
}