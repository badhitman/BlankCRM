////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Rows for warehouse document delete
/// </summary>
public class RowsForWarehouseDocumentDeleteReceive(ICommerceService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<int[]?, RowsForWarehouseDocumentDeleteResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RowsDeleteFromWarehouseDocumentCommerceReceive;

    /// <inheritdoc/>
    public async Task<RowsForWarehouseDocumentDeleteResponseModel?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        RowsForWarehouseDocumentDeleteResponseModel res = await commRepo.RowsForWarehouseDocumentDeleteAsync(req, token);
        if (res.Success() && res.DocumentsUpdated is not null && res.DocumentsUpdated.Count != 0)
        {
            List<Task> tasks = [];
            foreach (KeyValuePair<int, DeliveryDocumentMetadataRecord> node in res.DocumentsUpdated)
            {
                tasks.Add(Task.Run(async () =>
                {
                    TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, typeof(int[]).Name, req, node.Key.ToString());
                    await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(node.Value.Rows), token);
                }, token));
            }
            if (tasks.Count != 0)
                await Task.WhenAll(tasks);
        }
        return res;
    }
}