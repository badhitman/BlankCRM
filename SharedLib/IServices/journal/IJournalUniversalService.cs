////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Journal Universal
/// </summary>
public partial interface IJournalUniversalService
{
    /// <summary>
    /// Получить метаданные документа
    /// </summary>
    public Task<DocumentFitModel> GetDocumentMetadataAsync(string document_name_or_id, int? projectId = null, CancellationToken token = default);

    /// <summary>
    /// Получить колонки документа по его имени
    /// </summary>
    public Task<TResponseModel<EntryAltStandardModel[]?>> GetColumnsForJournalAsync(string document_name_or_id, int? projectId = null, CancellationToken token = default);

    /// <summary>
    /// Получить порцию документов
    /// </summary>
    public Task<TPaginationResponseStandardModel<KeyValuePair<int, Dictionary<string, object>>>> SelectJournalPartAsync(SelectJournalPartRequestModel req, int? projectId, CancellationToken token = default);
}