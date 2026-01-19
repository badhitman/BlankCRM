////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить связь [таба/вкладки схемы документа] с [формой] 
/// </summary>
public class DeleteTabDocumentSchemeJoinFormConstructorReceive(IConstructorService conService, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeleteTabDocumentSchemeJoinFormRequestModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteTabDocumentSchemeJoinFormConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteTabDocumentSchemeJoinFormRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        ResponseBaseModel res = await conService.DeleteTabDocumentSchemeJoinFormAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
