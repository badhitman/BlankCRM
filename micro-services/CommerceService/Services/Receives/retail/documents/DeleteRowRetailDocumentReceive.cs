////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteRowRetailDocument
/// </summary>
public class DeleteRowRetailDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeleteRowRetailDocumentRequestModel?, TResponseModel<Guid?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRowDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<Guid?>?> ResponseHandleActionAsync(DeleteRowRetailDocumentRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<Guid?> res = await commRepo.DeleteRowRetailDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}