////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.Daichi;

/// <summary>
/// HealthCheckReceive
/// </summary>
public class HealthCheckReceive(IDaichiBusinessApiService daichiRepo)
    : IResponseReceive<object?, TResponseModel<List<RabbitMqManagementResponseModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.HealthCheckDaichiReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await daichiRepo.HealthCheckAsync(token);
    }
}