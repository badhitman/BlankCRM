////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Обновление расписания на конкретную дату
/// </summary>
public class CalendarScheduleUpdateReceive(ICommerceService commerceRepo, ILogger<CalendarScheduleUpdateReceive> loggerRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestModel<CalendarScheduleModelDB>?, TResponseModel<int>?>
{
    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CalendarScheduleUpdateCommerceReceive;

    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestModel<CalendarScheduleModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req));
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await commerceRepo.CalendarScheduleUpdateAsync(req, token);
    }
}