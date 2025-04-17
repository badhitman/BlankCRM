////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AttendancesRecordsByIssuesGetReceive
/// </summary>
public class AttendancesRecordsByIssuesGetReceive(ICommerceService commRepo) : IResponseReceive<OrdersByIssuesSelectRequestModel?, TResponseModel<RecordsAttendanceModelDB[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrdersAttendancesByIssuesGetReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<RecordsAttendanceModelDB[]>?> ResponseHandleActionAsync(OrdersByIssuesSelectRequestModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await commRepo.RecordsAttendancesByIssuesGetAsync(payload, token);
    }
}