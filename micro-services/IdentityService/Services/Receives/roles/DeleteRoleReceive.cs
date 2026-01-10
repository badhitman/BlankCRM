////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Удалить роль (если у роли нет пользователей).
/// </summary>
public class DeleteRoleReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<string?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRoleReceive;

    /// <summary>
    /// Добавить роль пользователю (включить пользователя в роль)
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req))
            throw new ArgumentNullException(nameof(req));
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await idRepo.DeleteRoleAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}