////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Создает (и отправляет) токен изменения адреса электронной почты для указанного пользователя.
/// </summary>
public class GenerateChangeEmailTokenReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<GenerateChangeEmailTokenRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GenerateChangeEmailTokenReceive;

    /// <summary>
    /// Создает (и отправляет) токен изменения адреса электронной почты для указанного пользователя.
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(GenerateChangeEmailTokenRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.UserId);
        ResponseBaseModel res = await idRepo.GenerateChangeEmailTokenAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}