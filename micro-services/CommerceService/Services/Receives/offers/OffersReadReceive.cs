////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OffersReadReceive
/// </summary>
public class OffersReadReceive(ICommerceService commerceRepo)
    : IResponseReceive<TAuthRequestStandardModel<int[]>?, TResponseModel<OfferModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OfferReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<OfferModelDB[]>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.OffersReadAsync(req, token);
    }
}