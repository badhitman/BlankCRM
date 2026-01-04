////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Индексирование данных
/// </summary>
public interface IFilesIndexing
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> IndexingFileAsync(StorageFileMiddleModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>> SpreadsheetDocumentGetIndexAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>> WordprocessingDocumentGetIndexAsync(TAuthRequestModel<int> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveTraceForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default);
}