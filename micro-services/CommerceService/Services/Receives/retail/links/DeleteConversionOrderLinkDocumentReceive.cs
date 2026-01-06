////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteConversionOrderLinkDocument
/// </summary>
public class DeleteConversionOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeleteConversionOrderLinkRetailDocumentsRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteConversionOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeleteConversionOrderLinkRetailDocumentsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        ResponseBaseModel res = await commRepo.DeleteConversionOrderLinkDocumentRetailAsync(req, token);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);

        if (req.OrderId > 0)
        {
            trace.TraceReceiverRecordId = req.OrderId;
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        }

        if (req.ConversionId > 0)
        {
            trace.TraceReceiverRecordId = req.ConversionId;
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        }

        return res;
    }
}