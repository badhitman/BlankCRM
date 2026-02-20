////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrderUpdateOrCreate
/// </summary>
public class OrderUpdateOrCreateReceive(ICommerceService commRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<OrderDocumentModelDB>?, DocumentNewVersionResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrderUpdateOrCreateCommerceReceive;

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<OrderDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        DocumentNewVersionResponseModel res = await commRepo.OrderUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}