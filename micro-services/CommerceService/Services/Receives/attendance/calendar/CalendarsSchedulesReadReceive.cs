////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CalendarsSchedulesReadReceive
/// </summary>
public class CalendarsSchedulesReadReceive(ICommerceService commerceRepo)
    : IResponseReceive<TAuthRequestStandardModel<int[]>?, TResponseModel<List<CalendarScheduleModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CalendarsSchedulesReadCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<List<CalendarScheduleModelDB>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await commerceRepo.CalendarsSchedulesReadAsync(payload, token);
    }
}