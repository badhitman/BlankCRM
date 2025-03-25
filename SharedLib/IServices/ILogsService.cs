////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// ILogsService
/// </summary>
public interface ILogsService
{
    /// <summary>
    /// Определить номер страницы для строки
    /// </summary>
    public Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRowAsync(TPaginationRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// LogsSelect
    /// </summary>
    public Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestModel<LogsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// MetadataLogs
    /// </summary>
    public Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default);
}