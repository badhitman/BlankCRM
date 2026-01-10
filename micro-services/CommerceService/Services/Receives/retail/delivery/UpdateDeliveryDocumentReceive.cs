////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateDeliveryDocument
/// </summary>
public class UpdateDeliveryDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeliveryDocumentRetailModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDeliveryDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeliveryDocumentRetailModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Id.ToString());
        ResponseBaseModel res = await commRepo.UpdateDeliveryDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}