////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Обновить пользователю поля: FirstName и LastName
/// </summary>
public class UpdateUserDetailsReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<IdentityDetailsModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateUserDetailsReceive;

    /// <summary>
    /// Обновить пользователю поля: FirstName и LastName
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IdentityDetailsModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req);
        ResponseBaseModel res = await idRepo.UpdateUserDetailsAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}