////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// AttendancesRecordsStatusChangeByHelpDeskIdReceive
/// </summary>
public class AttendancesRecordsStatusChangeByHelpDeskIdReceive(ICommerceService commRepo) : IResponseReceive<TAuthRequestModel<StatusChangeRequestModel>?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.OrdersAttendancesStatusesChangeByHelpDeskDocumentIdReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<bool>?> ResponseHandleActionAsync(TAuthRequestModel<StatusChangeRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commRepo.RecordsAttendancesStatusesChangeByHelpDeskIdAsync(req, token);
    }
}