////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Обновление расписания на конкретную дату
/// </summary>
public class CalendarScheduleUpdateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<CalendarScheduleModelDB>?, TResponseModel<int>?>
{
    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CalendarScheduleUpdateCommerceReceive;

    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(TAuthRequestStandardModel<CalendarScheduleModelDB>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload.Id);
        TResponseModel<int> res = await commerceRepo.CalendarScheduleUpdateAsync(req, token);
        if (trace.TraceReceiverRecordId <= 0)
            trace.TraceReceiverRecordId = res.Response;

        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}