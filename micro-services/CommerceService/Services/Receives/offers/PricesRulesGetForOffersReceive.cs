////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// PricesRulesGetForOffersReceive
/// </summary>
public class PricesRulesGetForOffersReceive(ICommerceService commerceRepo)
    : IResponseReceive<TAuthRequestStandardModel<int[]>?, TResponseModel<List<PriceRuleForOfferModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PricesRulesGetForOfferCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<PriceRuleForOfferModelDB>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.PricesRulesGetForOffersAsync(req, token);
    }
}