////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Установить блокировку пользователю
/// </summary>
public class SetLockUserReceive(IIdentityTools idRepo, ILogger<SetLockUserReceive> loggerRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<IdentityBooleanModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetLockUserReceive;

    /// <summary>
    /// Установить блокировку пользователю
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IdentityBooleanModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await idRepo.SetLockUserAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}