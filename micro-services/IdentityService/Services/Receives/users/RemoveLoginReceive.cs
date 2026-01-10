////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Пытается удалить предоставленную внешнюю информацию для входа из указанного userId
/// и возвращает флаг, указывающий, удалось ли удаление или нет
/// </summary>
public class RemoveLoginReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<RemoveLoginRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RemoveLoginForUserReceive;

    /// <summary>
    /// Пытается удалить предоставленную внешнюю информацию для входа из указанного userId
    /// и возвращает флаг, указывающий, удалось ли удаление или нет
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(RemoveLoginRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.UserId);
        ResponseBaseModel res = await idRepo.RemoveLoginForUserAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}