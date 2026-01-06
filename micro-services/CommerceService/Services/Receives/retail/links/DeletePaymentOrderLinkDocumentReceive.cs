////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
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
        ResponseBaseModel res = await commRepo.DeletePaymentOrderLinkDocumentAsync(req, token);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);

        if (req.OrderId > 0)
        {
            trace.TraceReceiverRecordId = req.OrderId;
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        }

        if (req.PaymentId > 0)
        {
            trace.TraceReceiverRecordId = req.PaymentId;
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        }

        return res;
    }
}