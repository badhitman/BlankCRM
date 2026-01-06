////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrderUpdateReceive
/// </summary>
public class OrderUpdateReceive(ICommerceService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<OrderDocumentModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrderUpdateCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(OrderDocumentModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id);
        TResponseModel<int> res = await commRepo.OrderUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}