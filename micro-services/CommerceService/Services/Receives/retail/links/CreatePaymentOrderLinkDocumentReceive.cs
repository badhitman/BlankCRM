////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreatePaymentOrderLinkDocument
/// </summary>
public class CreatePaymentOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<PaymentOrderRetailLinkModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreatePaymentOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(PaymentOrderRetailLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.PaymentDocumentId.ToString());
        TResponseModel<int> res = await commRepo.CreatePaymentOrderLinkDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.OrderDocumentId.ToString());
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        return res;
    }
}