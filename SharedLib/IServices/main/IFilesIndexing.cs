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
    public Task<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>> SpreadsheetDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>> WordprocessingDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ResponseBaseModel> SaveTraceForReceiverAsync(TraceReceiverRecord req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseStandardModel<TraceReceiverRecord>> TracesSelectAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel> req, CancellationToken token = default);
}