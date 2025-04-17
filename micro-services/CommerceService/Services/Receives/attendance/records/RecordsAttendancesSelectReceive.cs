////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Подбор записей (актуальных)
/// </summary>
public class RecordsAttendancesSelectReceive(ICommerceService commerceRepo)
    : IResponseReceive<TPaginationRequestAuthModel<RecordsAttendancesRequestModel>?, TPaginationResponseModel<RecordsAttendanceModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RecordsAttendancesSelectCommerceReceive;

    /// <summary>
    /// Подбор записей (актуальных)
    /// </summary>
    public async Task<TPaginationResponseModel<RecordsAttendanceModelDB>?> ResponseHandleActionAsync(TPaginationRequestAuthModel<RecordsAttendancesRequestModel>? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await commerceRepo.RecordsAttendancesSelectAsync(payload, token);
    }
}