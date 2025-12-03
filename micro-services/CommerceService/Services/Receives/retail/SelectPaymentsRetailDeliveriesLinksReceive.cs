////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectPaymentsRetailDeliveriesLinks
/// </summary>
public class SelectPaymentsRetailDeliveriesLinksReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectPaymentsRetailDeliveriesLinksRequestModel>?, TPaginationResponseModel<PaymentRetailDeliveryLinkModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectPaymentsDeliveriesLinksRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDeliveryLinkModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectPaymentsRetailDeliveriesLinksRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectPaymentsRetailDeliveriesLinksAsync(req, token);
    }
}