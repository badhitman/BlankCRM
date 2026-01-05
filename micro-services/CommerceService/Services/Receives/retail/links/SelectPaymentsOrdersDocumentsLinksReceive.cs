////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectPaymentsOrdersDocumentsLinks
/// </summary>
public class SelectPaymentsOrdersDocumentsLinksReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel>?, TPaginationResponseModel<PaymentOrderRetailLinkModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectPaymentsOrdersDocumentsLinksRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentOrderRetailLinkModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectPaymentsOrdersLinksRetailDocumentsRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectPaymentsOrdersDocumentsLinksAsync(req, token);
    }
}