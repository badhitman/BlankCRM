////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Бронь/Запись (услуги/аренда)
/// </summary>
public partial interface ICommerceService
{
    #region records
    /// <summary>
    /// Подбор записей (актуальных)
    /// </summary>
    public Task<TPaginationResponseModel<RecordsAttendanceModelDB>> RecordsAttendancesSelectAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Удалить запись/бронь
    /// </summary>
    public Task<ResponseBaseModel> RecordAttendanceDeleteAsync(TAuthRequestModel<int> orderId, CancellationToken token = default);

    /// <summary>
    /// Смена статуса записи/брони по идентификатору HelpDesk документа
    /// </summary>
    /// <remarks>
    /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
    /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
    /// </remarks>
    public Task<TResponseModel<bool>> RecordsAttendancesStatusesChangeByHelpDeskIdAsync(TAuthRequestModel<StatusChangeRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить брони/записи по HelpDesk
    /// </summary>
    public Task<TResponseModel<RecordsAttendanceModelDB[]>> RecordsAttendancesByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Создать пакет записей/броней
    /// </summary>
    public Task<ResponseBaseModel> RecordsAttendanceCreateAsync(TAuthRequestModel<CreateAttendanceRequestModel> workSchedules, CancellationToken token = default);
    #endregion

    /// <summary>
    /// Поиск доступных услуг/броней
    /// </summary>
    public Task<WorksFindResponseModel> WorkSchedulesFindAsync(WorkFindRequestModel req, int[]? organizationsFilter = null, CancellationToken token = default);

    /// <summary>
    /// Недельное расписание обновить/создать
    /// </summary>
    public Task<TResponseModel<int>> WeeklyScheduleUpdateAsync(WeeklyScheduleModelDB work, CancellationToken token = default);

    /// <summary>
    /// Подбор недельных расписаний
    /// </summary>
    public Task<TPaginationResponseModel<WeeklyScheduleModelDB>> WeeklySchedulesSelectAsync(TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// Прочитать недельные расписания (по идентификаторам)
    /// </summary>
    public Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Расписание на дату создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> CalendarScheduleUpdateAsync(TAuthRequestModel<CalendarScheduleModelDB> work, CancellationToken token = default);

    /// <summary>
    /// Подбор расписаний для дат
    /// </summary>
    public Task<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>> CalendarSchedulesSelectAsync(TAuthRequestModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Расписания для дат прочитать по их иддентификаторам
    /// </summary>
    public Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarSchedulesReadAsync(TAuthRequestModel<int[]> req, CancellationToken token = default);
}