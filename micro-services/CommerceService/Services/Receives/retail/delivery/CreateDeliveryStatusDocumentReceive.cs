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
    : IResponseReceive<TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB>?, CreateDeliveryStatusDocumentResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<CreateDeliveryStatusDocumentResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeliveryStatusRetailDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        CreateDeliveryStatusDocumentResponseModel res = await commRepo.CreateDeliveryStatusDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}