////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Rusklimat;

/// <summary>
/// HealthCheckReceive
/// </summary>
public class HealthCheckReceive(IRusklimatComApiService rusklimatRepo)
    : IResponseReceive<object?, TResponseModel<List<RabbitMqManagementResponseModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.HealthCheckRusklimatReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await rusklimatRepo.HealthCheckAsync(token);
    }
}