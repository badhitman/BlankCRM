////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// SelectPaymentsOrdersLinks
/// </summary>
public class SelectPaymentsOrdersLinksReceive(IRetailService commRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SelectPaymentsRetailOrdersLinksRequestModel>?, TPaginationResponseModel<PaymentRetailOrderLinkModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectPaymentsOrdersLinksRetailReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<PaymentRetailOrderLinkModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SelectPaymentsRetailOrdersLinksRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.SelectPaymentsOrdersLinksAsync(req, token);
    }
}