////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectRowOfDeliveryDocuments
/// </summary>
public class SelectRowOfDeliveryDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel>?, TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectRowsOfDeliveryDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<RowOfDeliveryRetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectRowsOfDeliveriesRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectRowOfDeliveryDocumentsAsync(req, token);
    }
}