////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Хранилище параметров приложений
/// </summary>
/// <remarks>
/// Значения/данные сериализуются в JSON строку при сохранении и десерализируются при чтении
/// </remarks>
public interface ISerializeStorage
{
    #region logs
    /// <summary>
    /// Определить номер страницы для строки
    /// </summary>
    public Task<TPaginationResponseModel<NLogRecordModelDB>> GoToPageForRow(TPaginationRequestModel<int> req, CancellationToken token = default);

    /// <summary>
    /// Чтение логов
    /// </summary>
    public Task<TPaginationResponseModel<NLogRecordModelDB>> LogsSelect(TPaginationRequestModel<LogsSelectRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// MetadataLogs
    /// </summary>
    public Task<TResponseModel<LogsMetadataResponseModel>> MetadataLogs(PeriodDatesTimesModel req, CancellationToken token = default);
    #endregion

    #region tags
    /// <summary>
    /// FilesAreaGetMetadata
    /// </summary>
    public Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadata(FilesAreaMetadataRequestModel req, CancellationToken token = default);

    /// <summary>
    /// FilesSelect
    /// </summary>
    public Task<TPaginationResponseModel<StorageFileModelDB>> FilesSelect(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// ReadFile
    /// </summary>
    public Task<TResponseModel<FileContentModel>> ReadFile(TAuthRequestModel<RequestFileReadModel> req, CancellationToken token = default);

    /// <summary>
    /// SaveFile
    /// </summary>
    public Task<TResponseModel<StorageFileModelDB>> SaveFile(TAuthRequestModel<StorageImageMetadataModel> req, CancellationToken token = default);

    /// <summary>
    /// TagSet
    /// </summary>
    public Task<ResponseBaseModel> TagSet(TagSetModel req, CancellationToken token = default);

    /// <summary>
    /// TagsSelect
    /// </summary>
    public Task<TPaginationResponseModel<TagModelDB>> TagsSelect(TPaginationRequestModel<SelectMetadataRequestModel> req, CancellationToken token = default);
    #endregion

    #region storage parameters
    /// <summary>
    /// Сохранить параметр
    /// </summary>
    /// <typeparam name="T">Тип сохраняемых данных (сериализируемый)</typeparam>
    public Task Save<T>(T obj, StorageMetadataModel set, bool trimHistory = false, CancellationToken token = default);

    /// <summary>
    /// Прочитать значение параметра. null - если значения нет
    /// </summary>
    /// <typeparam name="T">Тип данных (для десериализации из JSON)</typeparam>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<T?> Read<T>(StorageMetadataModel req, CancellationToken token = default);

    /// <summary>
    /// Поиск значений параметров
    /// </summary>
    /// <typeparam name="T">Тип данных (для десериализации из JSON)</typeparam>
    public Task<T?[]> Find<T>(RequestStorageBaseModel req, CancellationToken token = default);

    /// <summary>
    /// FlushParameter
    /// </summary>
    public Task<TResponseModel<int?>> FlushParameter(StorageCloudParameterModelDB storage, bool trimHistory = false, CancellationToken token = default);

    /// <summary>
    /// Прочитать значение параметра. null - если значения нет
    /// </summary>
    /// <remarks>
    /// Возвращается самое актуальное значение (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<StorageCloudParameterPayloadModel>> ReadParameter(StorageMetadataModel req, CancellationToken token = default);

    /// <summary>
    /// Прочитать значения параметров. Данные запрашиваемых параметров, которые отсутствуют в БД - не попадут в возвращаемый ответ.
    /// </summary>
    /// <remarks>
    /// Возвращается самое актуальные значения (последнее установленное). Хранится история значений - если значение будет часто меняться будет ротация стека накопленных значений с усечением от 150 до 100.
    /// Проверка переполнения происходит при каждой команде сохранения.
    /// </remarks>
    public Task<TResponseModel<List<StorageCloudParameterPayloadModel>>> ReadParameters(StorageMetadataModel[] req, CancellationToken token = default);

    /// <summary>
    /// Поиск значений параметров
    /// </summary>
    public Task<TResponseModel<FoundParameterModel[]?>> Find(RequestStorageBaseModel req, CancellationToken token = default);
    #endregion
}