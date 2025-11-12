////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Serialize Remote Transmission Service
/// </summary>
public interface IStorageTransmission : IFilesStorage
{
    /// <summary>
    /// GoToPageForRow
    /// </summary>
    public Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRowAsync(TPaginationRequestStandardModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Metadata Logs
    /// </summary>
    public Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogsAsync(PeriodDatesTimesModel req, CancellationToken token = default);

    /// <summary>
    /// Чтение логов
    /// </summary>
    public Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelectAsync(TPaginationRequestStandardModel<LogsSelectRequestModel> req, CancellationToken token = default);
}