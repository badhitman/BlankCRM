////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateRowRetailDocument
/// </summary>
public class UpdateRowRetailDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<RowOfRetailOrderDocumentModelDB?, TResponseModel<Guid?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateRowDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>?> ResponseHandleActionAsync(RowOfRetailOrderDocumentModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<Guid?> res = await commRepo.UpdateRowRetailDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}