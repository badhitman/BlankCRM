////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Удалить Identity данные пользователя
/// </summary>
public class DeleteUserDataReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<DeleteUserDataRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.DeleteUserDataReceive;

    /// <summary>
    /// Удалить Identity данные пользователя
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(DeleteUserDataRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, new { req.UserId }, req.UserId);
        ResponseBaseModel res = await idRepo.DeleteUserDataAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}