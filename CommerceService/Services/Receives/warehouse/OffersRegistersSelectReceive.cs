////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OffersRegistersSelectReceive
/// </summary>
public class OffersRegistersSelectReceive(ICommerceService commRepo) : IResponseReceive<TPaginationRequestModel<RegistersSelectRequestBaseModel>?, TPaginationResponseModel<OfferAvailabilityModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.OffersRegistersSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OfferAvailabilityModelDB>?> ResponseHandleActionAsync(TPaginationRequestModel<RegistersSelectRequestBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.RegistersSelect(req, token);
    }
}