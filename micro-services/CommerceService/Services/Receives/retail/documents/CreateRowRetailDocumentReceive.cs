////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateRowRetailDocument
/// </summary>
public class CreateRowRetailDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB>?, TResponseModel<KeyValuePair<int, Guid>?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateRowDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<KeyValuePair<int, Guid>?>?> ResponseHandleActionAsync(TAuthRequestStandardModel<RowOfRetailOrderDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<KeyValuePair<int, Guid>?> res = await commRepo.CreateRowRetailDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}