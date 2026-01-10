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
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.DeliveryDocumentId.ToString());
        TResponseModel<int> res = await commRepo.CreateDeliveryOrderLinkDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.OrderDocumentId.ToString());
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);

        return res;
    }
}