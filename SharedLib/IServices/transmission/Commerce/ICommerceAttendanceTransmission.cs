////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ICommerceTransmission
/// </summary>
public partial interface ICommerceTransmission
{
    /// <summary>
    /// Подбор записей (актуальных)
    /// </summary>
    public Task<TPaginationResponseModel<RecordsAttendanceModelDB>> RecordsAttendancesSelectAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// AttendanceRecordsDelete
    /// </summary>
    public Task<ResponseBaseModel> AttendanceRecordsDeleteAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Смена статуса заявки (бронь)
    /// </summary>
    public Task<TResponseModel<bool>> StatusesOrdersAttendancesChangeByHelpdeskDocumentIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Получить заказы (по заявкам)
    /// </summary>
    public Task<TResponseModel<RecordsAttendanceModelDB[]>> OrdersAttendancesByIssuesAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Create attendance records
    /// </summary>
    public Task<ResponseBaseModel> CreateAttendanceRecordsAsync(TAuthRequestModel<CreateAttendanceRequestModel> workSchedules, CancellationToken token = default);

    /// <summary>
    /// OrganizationOfferContractUpdate
    /// </summary>
    public Task<TResponseModel<bool>> OrganizationOfferContractUpdateAsync(TAuthRequestModel<OrganizationOfferToggleModel> req, CancellationToken token = default);

    /// <summary>
    /// WorkSchedulesFind
    /// </summary>
    public Task<WorksFindResponseModel> WorksSchedulesFindAsync(WorkFindRequestModel req, CancellationToken token = default);

    /// <summary>
    /// WorkScheduleUpdate
    /// </summary>
    public Task<TResponseModel<int>> WeeklyScheduleUpdateAsync(WeeklyScheduleModelDB work, CancellationToken token = default);

    /// <summary>
    /// WorkSchedulesSelect select
    /// </summary>
    public Task<TPaginationResponseModel<WeeklyScheduleModelDB>> WeeklySchedulesSelectAsync(TPaginationRequestModel<WorkSchedulesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// WorkSchedulesRead read
    /// </summary>
    public Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// WorkScheduleCalendarUpdate
    /// </summary>
    public Task<TResponseModel<int>> CalendarScheduleUpdateAsync(TAuthRequestModel<CalendarScheduleModelDB> work, CancellationToken token = default);

    /// <summary>
    /// WorkScheduleCalendarsSelect
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>> CalendarsSchedulesSelectAsync(TAuthRequestModel<TPaginationRequestModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// WorkScheduleCalendarsRead
    /// </summary>
    public Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarsSchedulesReadAsync(TAuthRequestModel<int[]> req, CancellationToken token = default);
}