////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateDeliveryDocument
/// </summary>
public class CreateDeliveryDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<CreateDeliveryDocumentRetailRequestModel?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(CreateDeliveryDocumentRetailRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        TResponseModel<int> res = await commRepo.CreateDeliveryDocumentAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}