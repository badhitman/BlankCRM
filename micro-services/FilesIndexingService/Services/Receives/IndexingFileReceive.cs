////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// Indexing file
/// </summary>
public class IndexingFileReceive(ILogger<IndexingFileReceive> LoggerRepo, IFilesIndexing indexingFileRepo)
    : IResponseReceive<StorageFileMiddleModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IndexingFileReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(StorageFileMiddleModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");
        return await indexingFileRepo.IndexingFileAsync(req, token);
    }
}