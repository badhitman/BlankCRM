////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Подбор записей (актуальных)
/// </summary>
public class RecordsAttendancesSelectReceive(ICommerceService commerceRepo)
    : IResponseReceive<TPaginationRequestAuthModel<RecordsAttendancesRequestModel>?, TPaginationResponseStandardModel<RecordsAttendanceModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RecordsAttendancesSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<RecordsAttendanceModelDB>?> ResponseHandleActionAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await commerceRepo.RecordsAttendancesSelectAsync(payload, token);
    }
}