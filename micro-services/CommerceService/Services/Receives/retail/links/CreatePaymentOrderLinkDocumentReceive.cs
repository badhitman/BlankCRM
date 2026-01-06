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
        TResponseModel<int> res = await commRepo.CreatePaymentOrderLinkDocumentAsync(req, token);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.PaymentDocumentId);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);

        trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.OrderDocumentId);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);

        return res;
    }
}