////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteDeliveryStatusDocument
/// </summary>
public class DeleteDeliveryStatusDocumentReceive(IRetailService commRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeleteDeliveryStatusDocumentRequestModel>?, DeleteDeliveryStatusDocumentResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDeliveryStatusDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<DeleteDeliveryStatusDocumentResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteDeliveryStatusDocumentRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        DeleteDeliveryStatusDocumentResponseModel res = await commRepo.DeleteDeliveryStatusDocumentAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}