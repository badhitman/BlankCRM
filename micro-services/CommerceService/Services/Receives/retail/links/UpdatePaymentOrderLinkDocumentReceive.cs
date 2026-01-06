////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdatePaymentOrderLinkDocument
/// </summary>
public class UpdatePaymentOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<PaymentOrderRetailLinkModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdatePaymentOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(PaymentOrderRetailLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await commRepo.UpdatePaymentOrderLinkDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}