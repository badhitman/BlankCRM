////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class IndexingTransmission(IRabbitClient rabbitClient) : IIndexingServive
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IndexingFileAsync(StorageFileMiddleModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.IndexingFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>> SpreadsheetDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SpreadsheetDocumentIndexingFileResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SpreadsheetDocumentGetIndexFileReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>> WordprocessingDocumentGetIndexAsync(TAuthRequestStandardModel<int> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<WordprocessingDocumentIndexingFileResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.WordprocessingDocumentGetIndexFileReceive, req, token: token) ?? new();
}