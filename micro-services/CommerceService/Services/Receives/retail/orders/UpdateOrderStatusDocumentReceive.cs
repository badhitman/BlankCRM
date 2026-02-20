////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateOrderStatusDocument
/// </summary>
public class UpdateOrderStatusDocumentReceive(IRetailService commRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<OrderStatusRetailDocumentModelDB>?, TResponseModel<Guid?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateOrderStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>?> ResponseHandleActionAsync(TAuthRequestStandardModel<OrderStatusRetailDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<Guid?> res = await commRepo.UpdateOrderStatusDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}