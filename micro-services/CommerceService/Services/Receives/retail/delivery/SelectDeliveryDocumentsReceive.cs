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
    : IResponseReceive<TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel>?, TPaginationResponseStandardModel<DeliveryDocumentRetailModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectDeliveryDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<DeliveryDocumentRetailModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectDeliveryDocumentsRetailRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectDeliveryDocumentsAsync(req, token);
    }
}