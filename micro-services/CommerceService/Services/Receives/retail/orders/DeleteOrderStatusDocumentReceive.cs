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
    : IResponseReceive<TAuthRequestStandardModel<DeleteOrderStatusDocumentRequestModel>?, DocumentNewVersionResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteOrderStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteOrderStatusDocumentRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        DocumentNewVersionResponseModel res = await commRepo.DeleteOrderStatusDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}