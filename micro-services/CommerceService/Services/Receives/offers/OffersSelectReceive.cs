////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OffersSelectReceive
/// </summary>
public class OffersSelectReceive(ICommerceService commerceRepo)
    : IResponseReceive<TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>>?, TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OfferSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<OfferModelDB>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<OffersSelectRequestModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.OffersSelectAsync(req, token);
    }
}