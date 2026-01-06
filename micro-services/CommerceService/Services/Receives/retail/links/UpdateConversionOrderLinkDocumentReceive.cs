////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateConversionOrderLinkDocument
/// </summary>
public class UpdateConversionOrderLinkDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<ConversionOrderRetailLinkModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateConversionOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(ConversionOrderRetailLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await commRepo.UpdateConversionOrderLinkDocumentRetailAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}