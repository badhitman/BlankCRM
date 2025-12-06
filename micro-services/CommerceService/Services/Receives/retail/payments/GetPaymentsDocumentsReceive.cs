////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// GetPaymentsDocuments
/// </summary>
public class GetPaymentsDocumentsReceive(IRetailService commRepo)
    : IResponseReceive<GetPaymentsRetailOrdersDocumentsRequestModel?, TResponseModel<PaymentRetailDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetPaymentsDocumentsRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentRetailDocumentModelDB[]>?> ResponseHandleActionAsync(GetPaymentsRetailOrdersDocumentsRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.GetPaymentsDocumentsAsync(req, token);
    }
}