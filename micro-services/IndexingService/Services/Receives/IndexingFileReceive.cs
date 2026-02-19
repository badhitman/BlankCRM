////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// Indexing file
/// </summary>
public class IndexingFileReceive(IIndexingServive indexingFileRepo)
    : IResponseReceive<StorageFileMiddleModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.IndexingFileReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(StorageFileMiddleModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await indexingFileRepo.IndexingFileAsync(req, token);
    }
}