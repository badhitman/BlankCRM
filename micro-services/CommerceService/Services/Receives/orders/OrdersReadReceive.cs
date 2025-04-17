////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrdersReadReceive
/// </summary>
public class OrdersReadReceive(ICommerceService commRepo)
    : IResponseReceive<TAuthRequestModel<int[]>?, TResponseModel<OrderDocumentModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrdersReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<OrderDocumentModelDB[]>?> ResponseHandleActionAsync(TAuthRequestModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.OrdersReadAsync(req, token);
    }
}