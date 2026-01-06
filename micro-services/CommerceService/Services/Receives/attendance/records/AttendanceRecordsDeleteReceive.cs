////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AttendanceRecordsDeleteReceive
/// </summary>
public class AttendanceRecordsDeleteReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<int>?, ResponseBaseModel?>
{
    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AttendanceRecordDeleteCommerceReceive;

    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<int>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload);
        ResponseBaseModel res = await commerceRepo.RecordAttendanceDeleteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}