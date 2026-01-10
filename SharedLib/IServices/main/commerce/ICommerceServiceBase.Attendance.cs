////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <inheritdoc/>
public partial interface ICommerceServiceBase
{
    /// <summary>
    /// Создать пакет записей/броней
    /// </summary>
    /// <remarks>
    /// Бронирует слоты (свободные)
    /// </remarks>
    public Task<ResponseBaseModel> CreateAttendanceRecordsAsync(TAuthRequestStandardModel<CreateAttendanceRequestModel> workSchedules, CancellationToken token = default);

    /// <summary>
    /// Получить заказы (по заявкам)
    /// </summary>
    public Task<TResponseModel<RecordsAttendanceModelDB[]>> RecordsAttendancesByIssuesGetAsync(OrdersByIssuesSelectRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Удалить запись/бронь
    /// </summary>
    public Task<TResponseModel<RecordsAttendanceModelDB[]>> AttendanceRecordsDeleteAsync(TAuthRequestStandardModel<int[]> orderId, CancellationToken token = default);

    /// <summary>
    /// Подбор записей (актуальных)
    /// </summary>
    public Task<TPaginationResponseStandardModel<RecordsAttendanceModelDB>> RecordsAttendancesSelectAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel> req, CancellationToken token = default);

}