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
    : IResponseReceive<TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel>?, TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDeliveriesOrdersLinksDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RetailOrderDeliveryLinkModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectDeliveriesOrdersLinksRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectDeliveriesOrdersLinksDocumentsAsync(req, token);
    }
}