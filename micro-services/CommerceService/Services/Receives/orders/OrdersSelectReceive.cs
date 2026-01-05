////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrdersSelectReceive
/// </summary>
public class OrdersSelectReceive(ICommerceService commRepo) : IResponseReceive<TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>>?, TPaginationResponseStandardModel<OrderDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrdersSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<OrderDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<TAuthRequestStandardModel<OrdersSelectRequestModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.OrdersSelectAsync(req, token);
    }
}