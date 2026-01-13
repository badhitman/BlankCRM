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
    : IResponseReceive<TAuthRequestStandardModel<int[]>?, TResponseModel<Dictionary<int, DeliveryDocumentMetadataRecord>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RowsDeleteFromWarehouseDocumentCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<Dictionary<int, DeliveryDocumentMetadataRecord>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<Dictionary<int, DeliveryDocumentMetadataRecord>> res = await commRepo.RowsDeleteFromWarehouseDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}