////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectOrderDocumentStatuses
/// </summary>
public class SelectOrderDocumentStatusesReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel>?, TPaginationResponseStandardModel<OrderStatusRetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectOrderDocumentStatusesRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OrderStatusRetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectOrderStatusesRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectOrderDocumentStatusesAsync(req, token);
    }
}