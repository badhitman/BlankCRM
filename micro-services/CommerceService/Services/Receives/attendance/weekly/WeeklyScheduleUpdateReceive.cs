////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.commerce;

/// <summary>
/// Обновление WeeklyScheduleUpdateReceive
/// </summary>
public class WeeklyScheduleUpdateReceive(ICommerceService commerceRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<WeeklyScheduleModelDB?, TResponseModel<int>?>
{
    /// <summary>
    /// Обновление WorkSchedule
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.WeeklyScheduleUpdateCommerceReceive;

    /// <summary>
    /// Обновление WorkSchedule
    /// </summary>
    public async Task<TResponseModel<int>?> ResponseHandleActionAsync(WeeklyScheduleModelDB? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, req.GetType().Name, JsonConvert.SerializeObject(req));
        TResponseModel<int> res = await commerceRepo.WeeklyScheduleUpdateAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}