////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeletePaymentOrderLinkDocument
/// </summary>
public class DeletePaymentOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeletePaymentOrderLinkRetailDocumentsRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeletePaymentOrderLinkDocumentReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeletePaymentOrderLinkRetailDocumentsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await commRepo.DeletePaymentOrderLinkDocumentAsync(req, token);

        if (req.OrderId > 0)
        {
            trace.TraceReceiverRecordId = req.OrderId.ToString();
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        }

        if (req.PaymentId > 0)
        {
            trace.TraceReceiverRecordId = req.PaymentId.ToString();
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        }

        return res;
    }
}