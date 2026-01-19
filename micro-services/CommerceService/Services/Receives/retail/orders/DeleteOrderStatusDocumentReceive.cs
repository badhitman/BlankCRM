////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteOrderStatusDocument
/// </summary>
public class DeleteOrderStatusDocumentReceive(IRetailService commRepo, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeleteOrderStatusDocumentRequestModel>?, TResponseModel<DocumentNewVersionResponseModel?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteOrderStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentNewVersionResponseModel?>?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteOrderStatusDocumentRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<DocumentNewVersionResponseModel?> res = await commRepo.DeleteOrderStatusDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}