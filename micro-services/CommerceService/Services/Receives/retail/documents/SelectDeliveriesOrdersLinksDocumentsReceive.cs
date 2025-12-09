////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectDeliveriesOrdersLinksDocuments
/// </summary>
public class SelectDeliveriesOrdersLinksDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel>?, TPaginationResponseModel<RetailDeliveryOrderLinkModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDeliveriesOrdersLinksDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RetailDeliveryOrderLinkModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectDeliveriesOrdersLinksDocumentsAsync(req, token);
    }
}