////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// DeliveryServicesGet
/// </summary>
public class DeliveryServicesGetReceive(IRetailService commRepo)
    : IResponseReceive<int[]?, TResponseModel<DeliveryServiceRetailModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeliveryServicesGetRetailReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<DeliveryServiceRetailModelDB[]>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.DeliveryServicesGetAsync(req, token);
    }
}