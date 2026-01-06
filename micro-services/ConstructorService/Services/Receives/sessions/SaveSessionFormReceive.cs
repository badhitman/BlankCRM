////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Сохранить данные формы документа из сессии
/// </summary>
public class SaveSessionFormReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<SaveConstructorSessionRequestModel?, TResponseModel<ValueDataForSessionOfDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SaveSessionFormReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>?> ResponseHandleActionAsync(SaveConstructorSessionRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<ValueDataForSessionOfDocumentModelDB[]> res = await conService.SaveSessionFormAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
