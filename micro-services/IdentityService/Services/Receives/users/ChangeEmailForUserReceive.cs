////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Обновляет адрес Email, если токен действительный для пользователя.
/// </summary>
public class ChangeEmailForUserReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<IdentityEmailTokenModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ChangeEmailForUserReceive;

    /// <summary>
    /// Обновляет адрес Email, если токен действительный для пользователя.    
    /// Пользователь, адрес электронной почты которого необходимо обновить.Новый адрес электронной почты.Измененный токен электронной почты, который необходимо подтвердить.
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IdentityEmailTokenModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.UserId);
        ResponseBaseModel res = await idRepo.ChangeEmailAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}