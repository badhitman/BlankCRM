////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OfferUpdateOrCreate
/// </summary>
public class OfferUpdateOrCreateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<OfferModelDB>?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OfferUpdateOrCreateCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestStandardModel<OfferModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<int> res = await commerceRepo.OfferUpdateOrCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}