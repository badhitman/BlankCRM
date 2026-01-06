////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateConversionOrderLinkDocument
/// </summary>
public class CreateConversionOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<ConversionOrderRetailLinkModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateConversionOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(ConversionOrderRetailLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TResponseModel<int> res = await commRepo.CreateConversionOrderLinkDocumentRetailAsync(req, token);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.ConversionDocumentId);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);

        trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.OrderDocumentId);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);

        return res;
    }
}