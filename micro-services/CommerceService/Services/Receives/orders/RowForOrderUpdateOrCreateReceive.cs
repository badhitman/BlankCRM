////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// RowForOrderUpdateOrCreate
/// </summary>
public class RowForOrderUpdateOrCreateReceive(ICommerceService commRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<RowOfOrderDocumentModelDB>?, DocumentNewVersionResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RowForOrderUpdateOrCreateCommerceReceive;

    /// <inheritdoc/>
    public async Task<DocumentNewVersionResponseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<RowOfOrderDocumentModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        DocumentNewVersionResponseModel res = await commRepo.RowForOrderUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}