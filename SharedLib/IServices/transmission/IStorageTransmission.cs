////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Serialize Remote Transmission Service
/// </summary>
public interface IStorageTransmission
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

    /// <summary>
    /// Получить сводку (метаданные) по пространствам хранилища
    /// </summary>
    /// <remarks>
    /// Общий размер и количество группируется по AppName
    /// </remarks>
    public Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadataAsync(FilesAreaMetadataRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Files select
    /// </summary>
    public Task<TPaginationResponseModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// ReadFile
    /// </summary>
    public Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestModel<RequestFileReadModel> req, CancellationToken token = default);

    /// <summary>
    /// Сохранить файл
    /// </summary>
    public Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestModel<StorageImageMetadataModel> req, CancellationToken token = default);
}