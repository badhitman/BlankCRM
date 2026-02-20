////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Хранилище файлов приложений
/// </summary>
public interface IFilesStorage
{
    /// <summary>
    /// Получить сводку (метаданные) по пространствам хранилища
    /// </summary>
    /// <remarks>
    /// Общий размер и количество группируется по AppName
    /// </remarks>
    public Task<TResponseModel<FilesAreaMetadataModel[]>> FilesAreaGetMetadataAsync(FilesAreaMetadataRequestModel req, CancellationToken token = default);

    /// <summary>
    /// FilesSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// ReadFile
    /// </summary>
    public Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestStandardModel<RequestFileReadModel> req, CancellationToken token = default);

    /// <summary>
    /// Save file into storage
    /// </summary>
    public Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestStandardModel<StorageFileMetadataModel> req, CancellationToken token = default);

    /// <summary>
    /// Информация о директории на сервере
    /// </summary>
    /// <remarks>
    /// Файлы и папки внутри указанного пути
    /// </remarks>
    public Task<TResponseModel<DirectoryReadResponseModel>> GetDirectoryInfoAsync(DirectoryReadRequestModel req, CancellationToken token = default);

    /// <summary>
    /// Прочитать массив байт слева и справа от указанной точки указанного размера в байтах
    /// </summary>
    public Task<TResponseModel<Dictionary<DirectionsEnum, byte[]>>> ReadFileDataAboutPositionAsync(ReadFileDataAboutPositionRequestModel req, CancellationToken token = default);
}