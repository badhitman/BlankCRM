////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить сессию опроса/анкеты
/// </summary>
public class DeleteSessionDocumentConstructorReceive(IConstructorService conService, ITracesIndexing indexingRepo)
    : IResponseReceive<DeleteSessionDocumentRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteSessionDocumentConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeleteSessionDocumentRequestModel? payload, CancellationToken token = default)
    {
        if (payload is null)
            throw new ArgumentNullException(nameof(payload));

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, null, payload);
        ResponseBaseModel res = await conService.DeleteSessionDocumentAsync(payload, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}