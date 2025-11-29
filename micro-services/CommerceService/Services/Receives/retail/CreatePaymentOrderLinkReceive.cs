////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreatePaymentOrderLink
/// </summary>
public class CreatePaymentOrderLinkReceive(IRetailService commRepo)
    : IResponseReceive<PaymentRetailOrderLinkModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreatePaymentOrderLinkRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(PaymentRetailOrderLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.CreatePaymentOrderLinkAsync(req, token);
    }
}