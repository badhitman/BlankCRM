////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateDeliveryOrderLinkDocument
/// </summary>
public class CreateDeliveryOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<RetailOrderDeliveryLinkModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(RetailOrderDeliveryLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TResponseModel<int> res = await commRepo.CreateDeliveryOrderLinkDocumentAsync(req, token);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.DeliveryDocumentId);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);

        trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.OrderDocumentId);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);

        return res;
    }
}