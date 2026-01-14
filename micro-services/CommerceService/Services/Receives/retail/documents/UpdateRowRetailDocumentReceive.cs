////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateRowRetailDocument
/// </summary>
public class UpdateRowRetailDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB>?, TResponseModel<DocumentRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateRowDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DocumentRetailModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<DocumentRetailModelDB> res = await commRepo.UpdateRowRetailDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}