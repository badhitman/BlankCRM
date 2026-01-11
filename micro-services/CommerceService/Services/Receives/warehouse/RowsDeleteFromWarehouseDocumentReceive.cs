////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Rows for warehouse document delete
/// </summary>
public class RowsDeleteFromWarehouseDocumentReceive(ICommerceService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<int[]?, RowsForWarehouseDocumentDeleteResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RowsDeleteFromWarehouseDocumentCommerceReceive;

    /// <inheritdoc/>
    public async Task<RowsForWarehouseDocumentDeleteResponseModel?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        RowsForWarehouseDocumentDeleteResponseModel res = await commRepo.RowsDeleteFromWarehouseDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}