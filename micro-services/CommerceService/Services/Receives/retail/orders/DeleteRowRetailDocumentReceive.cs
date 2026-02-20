////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteRowRetailDocument
/// </summary>
public class DeleteRowRetailDocumentReceive(IRetailService commRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<DeleteRowRetailDocumentRequestModel>?, DeleteRowRetailDocumentResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRowDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<DeleteRowRetailDocumentResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<DeleteRowRetailDocumentRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        DeleteRowRetailDocumentResponseModel res = await commRepo.DeleteRowRetailDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}