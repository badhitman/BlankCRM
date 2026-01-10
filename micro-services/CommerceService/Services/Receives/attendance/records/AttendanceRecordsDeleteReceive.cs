////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AttendanceRecordsDelete
/// </summary>
public class AttendanceRecordsDeleteReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<int[]>?, TResponseModel<RecordsAttendanceModelDB[]>?>
{
    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AttendanceRecordDeleteCommerceReceive;

    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);

        TResponseModel<RecordsAttendanceModelDB[]> res = await commerceRepo.AttendanceRecordsDeleteAsync(req, token);
        List<Task> tasks = [];
        if (res.Response is null || res.Response.Length == 0 || !res.Success())
            tasks.Add(Task.Run(async () =>
            {
                TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
                await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
            }, token));
        else
        {
            foreach (IGrouping<int, RecordsAttendanceModelDB> offerChange in res.Response.GroupBy(x => x.OfferId))
            {
                tasks.Add(Task.Run(async () =>
                {
                    TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, offerChange.Key.ToString());
                    await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(offerChange), token);
                }, token));
            }
        }

        if (tasks.Count != 0)
            await Task.WhenAll(tasks);

        return res;
    }
}