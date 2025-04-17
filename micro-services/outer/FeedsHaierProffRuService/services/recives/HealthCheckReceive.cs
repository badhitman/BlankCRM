////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Outers.HaierProff;

/// <summary>
/// HealthCheckReceive
/// </summary>
public class HealthCheckReceive(IFeedsHaierProffRuService haierRepo)
    : IResponseReceive<object?, TResponseModel<List<RabbitMqManagementResponseModel>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.HealthCheckHaierProffReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<RabbitMqManagementResponseModel>>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {
        //ArgumentNullException.ThrowIfNull(payload);
        return await haierRepo.HealthCheckAsync(token);
    }
}