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
    public Task<TPaginationResponseModel<StorageFileModelDB>> FilesSelectAsync(TPaginationRequestStandardModel<SelectMetadataRequestModel> req, CancellationToken token = default);

    /// <summary>
    /// ReadFile
    /// </summary>
    public Task<TResponseModel<FileContentModel>> ReadFileAsync(TAuthRequestModel<RequestFileReadModel> req, CancellationToken token = default);

    /// <summary>
    /// Save file into storage
    /// </summary>
    public Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestModel<StorageFileMetadataModel> req, CancellationToken token = default);

    /// <summary>
    /// GetDirectoryInfo
    /// </summary>
    public Task<TResponseModel<DirectoryReadResponseModel>> GetDirectoryInfoAsync(DirectoryReadRequestModel req, CancellationToken token = default);
}