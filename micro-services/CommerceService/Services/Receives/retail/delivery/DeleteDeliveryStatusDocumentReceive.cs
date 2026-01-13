////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteDeliveryStatusDocument
/// </summary>
public class DeleteDeliveryStatusDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<int>?, TResponseModel<DeliveryDocumentRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDeliveryStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<DeliveryDocumentRetailModelDB> res = await commRepo.DeleteDeliveryStatusDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}