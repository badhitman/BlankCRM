////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// OrdersSelectReceive
/// </summary>
public class OrdersSelectReceive(ICommerceService commRepo) : IResponseReceive<TPaginationRequestStandardModel<TAuthRequestModel<OrdersSelectRequestModel>>?, TPaginationResponseModel<OrderDocumentModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrdersSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<OrderDocumentModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<TAuthRequestModel<OrdersSelectRequestModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.OrdersSelectAsync(req, token);
    }
}