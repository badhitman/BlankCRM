////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <inheritdoc/>
public class FileIndexingTransmission(IRabbitClient rabbitClient) : IFilesIndexing
{
    /// <inheritdoc/>
    public async Task<ResponseBaseModel> IndexingFileAsync(StorageFileMiddleModel req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.IndexingFileReceive, req, token: token) ?? new();
}