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
    : IResponseReceive<TAuthRequestStandardModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>>?, TResponseModel<TPaginationResponseStandardModel<CalendarScheduleModelDB>>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CalendarsSchedulesSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<TPaginationResponseStandardModel<CalendarScheduleModelDB>>?> ResponseHandleActionAsync(TAuthRequestStandardModel<TPaginationRequestStandardModel<WorkScheduleCalendarsSelectRequestModel>>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.CalendarsSchedulesSelectAsync(req, token);
    }
}