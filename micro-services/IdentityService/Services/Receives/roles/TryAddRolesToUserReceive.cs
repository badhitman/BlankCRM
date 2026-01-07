////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Попытка добавить роли пользователю. Если роли такой нет, то она будет создана.
/// </summary>
public class TryAddRolesToUserReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<UserRolesModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TryAddRolesToUserReceive;

    /// <summary>
    /// Попытка добавить роли пользователю. Если роли такой нет, то она будет создана.
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(UserRolesModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.UserId);
        ResponseBaseModel res = await idRepo.TryAddRolesToUserAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}