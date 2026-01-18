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
    : IResponseReceive<int, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteSessionDocumentConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, payload);
        ResponseBaseModel res = await conService.DeleteSessionDocumentAsync(payload, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}