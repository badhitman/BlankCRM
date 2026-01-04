////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// SaveTraceForReceiverHandle
/// </summary>
public class SaveTraceForReceiverHandle(ILogger<SaveTraceForReceiverHandle> LoggerRepo, IFilesIndexing indexingFileRepo)
    : IResponseReceive<TraceReceiverRecord?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SaveTraceForReceiverReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TraceReceiverRecord? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        LoggerRepo.LogDebug($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");
        return await indexingFileRepo.SaveTraceForReceiverAsync(req, token);
    }
}