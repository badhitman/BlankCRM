////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectDeliveryStatusesDocuments
/// </summary>
public class SelectDeliveryStatusesDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel>?, TPaginationResponseStandardModel<DeliveryStatusRetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDeliveryStatusesDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DeliveryStatusRetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectDeliveryStatusesDocumentsAsync(req, token);
    }
}