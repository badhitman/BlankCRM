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
    /// FilesAreaGetMetadata
    /// </summary>
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
    /// SaveFile
    /// </summary>
    public Task<TResponseModel<StorageFileModelDB>> SaveFileAsync(TAuthRequestModel<StorageImageMetadataModel> req, CancellationToken token = default);
}