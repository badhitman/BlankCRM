////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// PaymentsOrdersDocumentsLinksGet
/// </summary>
public class PaymentsOrdersDocumentsLinksGetReceive(IRetailService commRepo)
    : IResponseReceive<int[]?, TResponseModel<PaymentOrderRetailLinkModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.PaymentsOrdersDocumentsLinksGetReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<PaymentOrderRetailLinkModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.PaymentsOrdersDocumentsLinksGetAsync(req, token);
    }
}