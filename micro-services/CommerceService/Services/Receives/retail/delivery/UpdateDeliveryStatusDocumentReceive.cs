////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateDeliveryStatusDocument
/// </summary>
public class UpdateDeliveryStatusDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB>?, TResponseModel<DeliveryDocumentRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDeliveryStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryDocumentRetailModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<DeliveryDocumentRetailModelDB> res = await commRepo.UpdateDeliveryStatusDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}