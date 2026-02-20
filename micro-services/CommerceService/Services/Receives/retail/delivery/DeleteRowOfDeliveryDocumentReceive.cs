////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteRowOfDeliveryDocument
/// </summary>
public class DeleteRowOfDeliveryDocumentReceive(IRetailService commRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeleteRowOfDeliveryDocumentRequestModel>?, DocumentNewVersionResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRowOfDeliveryDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteRowOfDeliveryDocumentRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        DocumentNewVersionResponseModel res = await commRepo.DeleteRowOfDeliveryDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}