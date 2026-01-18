////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// PriceRuleDeleteReceive
/// </summary>
public class PriceRuleDeleteReceive(ICommerceService commerceRepo, ITracesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<int>?, TResponseModel<PriceRuleForOfferModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PriceRuleDeleteCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<PriceRuleForOfferModelDB>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<PriceRuleForOfferModelDB> res = await commerceRepo.PriceRuleDeleteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}