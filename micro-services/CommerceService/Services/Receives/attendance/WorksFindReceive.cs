////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// WorksFindReceive
/// </summary>
public class WorksFindReceive(ICommerceService commerceRepo) : IResponseReceive<WorkFindRequestModel?, WorksFindResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WorksSchedulesFindCommerceReceive;

    /// <inheritdoc/>
    public async Task<WorksFindResponseModel?> ResponseHandleActionAsync(WorkFindRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await commerceRepo.WorkSchedulesFindAsync(req, token: token);
    }
}