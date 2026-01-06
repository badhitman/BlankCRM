////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Обновить (или создать) сессию опроса/анкеты
/// </summary>
public class UpdateOrCreateSessionDocumentReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<SessionOfDocumentDataModelDB?, TResponseModel<SessionOfDocumentDataModelDB?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrCreateSessionDocumentReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SessionOfDocumentDataModelDB?>?> ResponseHandleActionAsync(SessionOfDocumentDataModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<SessionOfDocumentDataModelDB?> res = await conService.UpdateOrCreateSessionDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
