////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectDeliveryServices
/// </summary>
public class SelectDeliveryServicesReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel>?, TPaginationResponseModel<DeliveryServiceRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDeliveryServicesRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryServiceRetailModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectDeliveryServicesRetailRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectDeliveryServicesAsync(req, token);
    }
}