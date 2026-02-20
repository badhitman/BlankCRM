////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Индексирование данных
/// </summary>
public interface IIndexingServive
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> IndexingFileAsync(StorageFileMiddleModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>> SpreadsheetDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>> WordprocessingDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default);
}