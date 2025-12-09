////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// UpdateDeliveryOrderLinkDocument
/// </summary>
public class UpdateDeliveryOrderLinkDocumentReceive(IRetailService commRepo)
    : IResponseReceive<RetailDeliveryOrderLinkModelDB?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateDeliveryOrderLinkDocumentRetailReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(RetailDeliveryOrderLinkModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.UpdateDeliveryOrderLinkDocumentAsync(req, token);
    }
}