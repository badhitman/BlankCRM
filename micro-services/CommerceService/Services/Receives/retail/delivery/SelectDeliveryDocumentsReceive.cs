////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectDeliveryDocuments
/// </summary>
public class SelectDeliveryDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel>?, TPaginationResponseModel<DeliveryDocumentRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDeliveryDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<DeliveryDocumentRetailModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectDeliveryDocumentsAsync(req, token);
    }
}