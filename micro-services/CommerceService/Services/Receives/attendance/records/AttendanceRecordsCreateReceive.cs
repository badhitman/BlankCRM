////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AttendanceRecordsCreate
/// </summary>
public class AttendanceRecordsCreateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<CreateAttendanceRequestModel>?, ResponseBaseModel?>
{
    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateAttendanceRecordsCommerceReceive;

    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<CreateAttendanceRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await commerceRepo.RecordsAttendanceCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}