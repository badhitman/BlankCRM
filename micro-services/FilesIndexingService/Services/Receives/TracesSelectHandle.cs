////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.indexing;

/// <summary>
/// TracesSelect
/// </summary>
public class TracesSelectHandle(ILogger<TracesSelectHandle> LoggerRepo, IFilesIndexing indexingFileRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectTraceReceivesRequestModel>?, TPaginationResponseStandardModel<TraceReceiverRecord>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TracesSelectReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<TraceReceiverRecord>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectTraceReceivesRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await indexingFileRepo.TracesSelectAsync(req, token);
    }
}