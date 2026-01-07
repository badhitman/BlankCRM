////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Исключить пользователя из роли (лишить пользователя роли)
/// </summary>
public class DeleteRoleFromUserReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<RoleEmailModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteRoleFromUserReceive;

    /// <summary>
    /// Исключить пользователя из роли (лишить пользователя роли)
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(RoleEmailModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Email);
        ResponseBaseModel res = await idRepo.DeleteRoleFromUserAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}