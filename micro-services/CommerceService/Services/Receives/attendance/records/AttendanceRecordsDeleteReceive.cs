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
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req);
        TResponseModel<RecordsAttendanceModelDB[]> res = await commerceRepo.AttendanceRecordsDeleteAsync(req, token);

        if (res.Response is null || res.Response.Length == 0 || !res.Success())
            await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        else
        {
            foreach (IGrouping<int, RecordsAttendanceModelDB> offerChange in res.Response.GroupBy(x => x.OfferId))
                await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(offerChange.ToArray()), token);
        }

        return res;
    }
}