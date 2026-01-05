////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeliveriesOrdersLinksDocumentsRead
/// </summary>
public class DeliveriesOrdersLinksDocumentsReadReceive(IRetailService commRepo)
    : IResponseReceive<int[]?, TResponseModel<RetailOrderDeliveryLinkModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeliveriesOrdersLinksDocumentsReadRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<RetailOrderDeliveryLinkModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.DeliveriesOrdersLinksDocumentsReadAsync(req, token);
    }
}