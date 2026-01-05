////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AttendanceRecordsCreateReceive
/// </summary>
public class AttendanceRecordsCreateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestModel<CreateAttendanceRequestModel>?, ResponseBaseModel?>
{
    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CreateAttendanceRecordsCommerceReceive;

    /// <summary>
    /// Обновление WorkScheduleCalendar
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestModel<CreateAttendanceRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));
        ResponseBaseModel res = await commerceRepo.RecordsAttendanceCreateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}