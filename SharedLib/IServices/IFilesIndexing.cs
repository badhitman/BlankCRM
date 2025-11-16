////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Индексирование файлов
/// </summary>
public interface IFilesIndexing
{
    /// <summary>
    /// IndexingFileAsync
    /// </summary>
    public Task<ResponseBaseModel> IndexingFileAsync(StorageFileMiddleModel req, CancellationToken token = default);
}