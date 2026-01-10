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
    public async Task<TResponseModel<List<CalendarScheduleModelDB>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.CalendarsSchedulesReadAsync(req, token);
    }
}