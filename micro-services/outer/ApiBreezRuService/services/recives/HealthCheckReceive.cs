////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Breez;

/// <summary>
/// HealthCheckReceive
/// </summary>
public class HealthCheckReceive(IBreezRuApiService breezRepo)
    : IResponseReceive<object?, TResponseModel<List<RabbitMqManagementResponseModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.HealthCheckBreezReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await breezRepo.HealthCheckAsync(token);
    }
}