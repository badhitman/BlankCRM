////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateDeliveryStatusDocument
/// </summary>
public class CreateDeliveryStatusDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeliveryStatusRetailDocumentModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(DeliveryStatusRetailDocumentModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.DeliveryDocumentId.ToString());
        TResponseModel<int> res = await commRepo.CreateDeliveryStatusDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        return res;
    }
}