﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OffersSelectReceive
/// </summary>
public class OffersSelectReceive(ICommerceService commerceRepo)
    : IResponseReceive<TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>>?, TResponseModel<TPaginationResponseModel<OfferModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.OfferSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<OfferModelDB>>?> ResponseHandleActionAsync(TAuthRequestModel<TPaginationRequestModel<OffersSelectRequestModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.OffersSelect(req, token);
    }
}