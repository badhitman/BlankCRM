////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OffersRegistersSelectReceive
/// </summary>
public class OffersRegistersSelectReceive(ICommerceService commRepo) : IResponseReceive<TPaginationRequestStandardModel<RegistersSelectRequestBaseModel>?, TPaginationResponseModel<OfferAvailabilityModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OffersRegistersSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OfferAvailabilityModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<RegistersSelectRequestBaseModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.RegistersSelectAsync(req, token);
    }
}