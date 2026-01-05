////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeleteDeliveryOrderLinkDocument
/// </summary>
public class DeleteDeliveryOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeleteDeliveryOrderLinkRetailDocumentsRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteDeliveryOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeleteDeliveryOrderLinkRetailDocumentsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        ResponseBaseModel res = await commRepo.DeleteDeliveryOrderLinkDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}