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
    : IResponseReceive<TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel>?, TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDeliveryStatusesDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryStatusRetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectDeliveryStatusesRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectDeliveryStatusesDocumentsAsync(req, token);
    }
}