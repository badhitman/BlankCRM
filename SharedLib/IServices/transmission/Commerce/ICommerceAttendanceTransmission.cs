////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ICommerceTransmission
/// </summary>
public partial interface ICommerceTransmission : ICommerceServiceBase
{
    /// <summary>
    /// Смена статуса заявки (бронь)
    /// </summary>
    public Task<TResponseModel<bool>> StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// WorkSchedulesFind
    /// </summary>
    public Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, CancellationToken token = default);
}