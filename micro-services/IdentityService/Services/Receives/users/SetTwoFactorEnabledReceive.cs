////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Вкл/Выкл двухфакторную аутентификацию для указанного userId
/// </summary>
public class SetTwoFactorEnabledReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<SetTwoFactorEnabledRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetTwoFactorEnabledReceive;

    /// <summary>
    /// Вкл/Выкл двухфакторную аутентификацию для указанного userId
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(SetTwoFactorEnabledRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.UserId);
        ResponseBaseModel res = await idRepo.SetTwoFactorEnabledAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}