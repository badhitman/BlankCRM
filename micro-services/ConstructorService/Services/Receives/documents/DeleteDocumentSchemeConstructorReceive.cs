////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.constructor;

/// <summary>
/// Удалить схему документа
/// </summary>
public class DeleteDocumentSchemeConstructorReceive(IConstructorService conService, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeleteDocumentSchemeRequestModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDocumentSchemeConstructorReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteDocumentSchemeRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        ResponseBaseModel res = await conService.DeleteDocumentSchemeAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}
