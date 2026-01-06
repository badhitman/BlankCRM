////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteDeliveryOrderLinkDocument
/// </summary>
public class DeleteDeliveryOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeleteDeliveryOrderLinkRetailDocumentsRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDeliveryOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeleteDeliveryOrderLinkRetailDocumentsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await commRepo.DeleteDeliveryOrderLinkDocumentAsync(req, token);

        if (req.OrderId > 0)
        {
            trace.TraceReceiverRecordId = req.OrderId;
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        }

        if (req.DeliveryId > 0)
        {
            trace.TraceReceiverRecordId = req.DeliveryId;
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        }

        return res;
    }
}