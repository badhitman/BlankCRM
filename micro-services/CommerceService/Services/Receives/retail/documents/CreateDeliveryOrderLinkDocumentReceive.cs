////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CreateDeliveryOrderLinkDocument
/// </summary>
public class CreateDeliveryOrderLinkDocumentReceive(IRetailService commRepo)
    : IResponseReceive<RetailDeliveryOrderLinkModelDB?, TResponseModel<int>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateDeliveryOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(RetailDeliveryOrderLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.CreateDeliveryOrderLinkDocumentAsync(req, token);
    }
}