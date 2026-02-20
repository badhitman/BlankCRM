////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateRowOfDeliveryDocument
/// </summary>
public class CreateRowOfDeliveryDocumentReceive(IRetailService commRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB>?, DocumentNewVersionResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateRowOfDeliveryDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<RowOfDeliveryRetailDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        DocumentNewVersionResponseModel res = await commRepo.CreateRowOfDeliveryDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}