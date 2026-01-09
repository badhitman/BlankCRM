////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Бронь/Запись (услуги/аренда)
/// </summary>
public partial interface ICommerceService : ICommerceServiceCore
{
    #region records
    /// <summary>
    /// Смена статуса записи/брони по идентификатору HelpDesk документа
    /// </summary>
    /// <remarks>
    /// В запросе нельзя указывать идентификатор заказа: только идентификатор HelpDesk документа.
    /// Допускается ситуация, когда под одним идентификатором HelpDesk документа могут существовать несколько заказов (объединённые заказы).
    /// </remarks>
    public Task<TResponseModel<bool>> StatusesOrdersAttendancesChangeByHelpDeskDocumentIdAsync(TAuthRequestStandardModel<StatusChangeRequestModel> req, CancellationToken token = default);
#endregion

    /// <summary>
    /// Поиск доступных услуг/броней
    /// </summary>
    public Task<WorksFindResponseModel> WorkSchedulesFindAsync(WorkFindRequestModel req, int[]? organizationsFilter = null, CancellationToken token = default);

    /// <summary>
    /// Прочитать недельные расписания (по идентификаторам)
    /// </summary>
    public Task<List<WeeklyScheduleModelDB>> WeeklySchedulesReadAsync(int[] req, CancellationToken token = default);

    /// <summary>
    /// Расписание на дату создать/обновить
    /// </summary>
    public Task<TResponseModel<int>> CalendarScheduleUpdateOrCreateAsync(TAuthRequestStandardModel<CalendarScheduleModelDB> work, CancellationToken token = default);

    /// <summary>
    /// Подбор расписаний для дат
    /// </summary>
    public Task<TResponseModel<TPaginationResponseStandardModel<CalendarScheduleModelDB>>> CalendarSchedulesSelectAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>> req, CancellationToken token = default);

    /// <summary>
    /// Расписания для дат прочитать по их иддентификаторам
    /// </summary>
    public Task<TResponseModel<List<CalendarScheduleModelDB>>> CalendarSchedulesReadAsync(TAuthRequestStandardModel<int[]> req, CancellationToken token = default);
}