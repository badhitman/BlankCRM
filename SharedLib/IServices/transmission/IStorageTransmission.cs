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
    /// TagSet
    /// </summary>
    public Task<TResponseModel<bool>> TagSetAsync(TagSetModel req, CancellationToken token = default);

    /// <summary>
    /// TagsSelect
    /// </summary>
    public Task<TPaginationResponseModel<TagModelDB>> TagsSelectAsync(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default);

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
    public Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestModel<RequestFileReadModel>? req, CancellationToken token = default);

    /// <summary>
    /// Сохранить файл
    /// </summary>
    public Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestModel<StorageImageMetadataModel>? req, CancellationToken token = default);

    /// <summary>
    /// Прочитать значение параметра. null - если значения нет
    /// </summary>
    /// <typeparam name="T">Тип данных (для десериализации из JSON)</typeparam>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<T?>> ReadParameterAsync<T>(StorageMetadataModel req, CancellationToken token = default);

    /// <summary>
    /// Прочитать значения параметров. Данные запрашиваемых параметров, которые отсутствуют в БД - не попадут в возвращаемый ответ.
    /// </summary>
    /// <typeparam name="T">Тип данных (для десериализации из JSON)</typeparam>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<List<T>?>> ReadParametersAsync<T>(StorageMetadataModel[] req, CancellationToken token = default);

    /// <summary>
    /// Сохранить параметр
    /// </summary>
    public Task<TResponseModel<int>> SaveParameterAsync<T>(T payload_query, StorageMetadataModel store, bool trim, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Найти/подобрать значения параметров (со всей историей значений)
    /// </summary>
    public Task<TResponseModel<T?[]?>> FindParametersAsync<T>(RequestStorageBaseModel req, CancellationToken token = default);
}