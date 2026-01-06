////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateConversionDocument
/// </summary>
public class UpdateConversionDocumentReceive(IRetailService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<WalletConversionRetailDocumentModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateConversionDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(WalletConversionRetailDocumentModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await commRepo.UpdateConversionDocumentRetailAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}