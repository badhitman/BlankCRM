////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// ILogsService
/// </summary>
public interface ILogsService
{
    /// <summary>
    /// Определить номер страницы для строки
    /// </summary>
    public Task<TPaginationResponseStandardModel<NLogRecordModelDB>> GoToPageForRowLogsAsync(TPaginationRequestStandardModel<GoToPageForRowLogsRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// LogsSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// MetadataLogs
    /// </summary>
    public Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default);
}