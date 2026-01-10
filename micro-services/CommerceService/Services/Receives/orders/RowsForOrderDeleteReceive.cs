////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// RowsForOrderDeleteReceive
/// </summary>
public class RowsForOrderDeleteReceive(ICommerceService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<int[]?, TResponseModel<RowOrderDocumentRecord[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RowsDeleteFromOrderCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<RowOrderDocumentRecord[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<RowOrderDocumentRecord[]> res = await commRepo.RowsForOrderDeleteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}