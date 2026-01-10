////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// WeeklySchedulesSelectReceive
/// </summary>
public class WeeklySchedulesSelectReceive(ICommerceService commerceRepo)
    : IResponseReceive<TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel>?, TPaginationResponseStandardModel<WeeklyScheduleModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WeeklySchedulesSelectCommerceReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<WeeklyScheduleModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<WorkSchedulesSelectRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.WeeklySchedulesSelectAsync(req, token);
    }
}