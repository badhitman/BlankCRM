////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Сохранить данные формы документа из сессии
/// </summary>
public class SaveSessionFormConstructorReceive(IConstructorService conService, IFilesIndexing indexingRepo)
    : IResponseReceive<SaveConstructorSessionRequestModel?, TResponseModel<ValueDataForSessionOfDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SaveSessionFormConstructorReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<ValueDataForSessionOfDocumentModelDB[]>?> ResponseHandleActionAsync(SaveConstructorSessionRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.SessionId.ToString());
        TResponseModel<ValueDataForSessionOfDocumentModelDB[]> res = await conService.SaveSessionFormAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<ValueDataForSessionOfDocumentModelDB[]>)), token);
        return res;
    }
}
