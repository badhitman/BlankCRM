////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateRetailDocument
/// </summary>
public class CreateRetailDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<CreateDocumentRetailRequestModel?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(CreateDocumentRetailRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<int> res = await commRepo.CreateRetailDocumentAsync(req, token);
        trace.TraceReceiverRecordId = res.Response.ToString();
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, nameof(TResponseModel<int>)), token);
        return res;
    }
}