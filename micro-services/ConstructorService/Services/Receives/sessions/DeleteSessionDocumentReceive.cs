////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DocumentFormat.OpenXml.Drawing;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить сессию опроса/анкеты
/// </summary>
public class DeleteSessionDocumentReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<int, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteSessionDocumentReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(int payload, CancellationToken token = default)
    {
        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, payload.GetType().Name, payload.ToString());
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await conService.DeleteSessionDocumentAsync(payload, token);
    }
}