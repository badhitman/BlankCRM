////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// WorkScheduleReadReceive
/// </summary>
public class WeeklySchedulesReadReceive(ICommerceService commerceRepo) : IResponseReceive<int[]?, List<WeeklyScheduleModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WeeklySchedulesReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<List<WeeklyScheduleModelDB>?> ResponseHandleActionAsync(int[]? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.WeeklySchedulesReadAsync(req, token);
    }
}