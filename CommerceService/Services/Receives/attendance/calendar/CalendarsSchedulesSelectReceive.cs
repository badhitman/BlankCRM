////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// CalendarsSchedulesSelectReceive
/// </summary>
public class CalendarsSchedulesSelectReceive(ICommerceService commerceRepo)
    : IResponseReceive<TAuthRequestModel<TPaginationRequestModel<WorkScheduleCalendarsSelectRequestModel>>?, TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.CalendarsSchedulesSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseModel<CalendarScheduleModelDB>>?> ResponseHandleActionAsync(TAuthRequestModel<TPaginationRequestModel<WorkScheduleCalendarsSelectRequestModel>>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await commerceRepo.CalendarSchedulesSelect(payload, token);
    }
}