////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AttendanceRecordsDelete
/// </summary>
public class AttendanceRecordsDeleteReceive(ICommerceService commerceRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<int[]>?, TResponseModel<RecordsAttendanceModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.AttendanceRecordsDeleteCommerceReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>?> ResponseHandleActionAsync(TAuthRequestStandardModel<int[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<RecordsAttendanceModelDB[]> res = await commerceRepo.AttendanceRecordsDeleteAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}