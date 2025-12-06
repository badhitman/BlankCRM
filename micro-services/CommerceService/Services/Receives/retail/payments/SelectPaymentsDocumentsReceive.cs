////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectPaymentsDocuments
/// </summary>
public class SelectPaymentsDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel>?, TPaginationResponseModel<PaymentRetailDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectPaymentsDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectPaymentsDocumentsAsync(req, token);
    }
}