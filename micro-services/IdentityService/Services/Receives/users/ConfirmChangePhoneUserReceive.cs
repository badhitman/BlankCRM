////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// ConfirmChangePhoneUser
/// </summary>
public class ConfirmChangePhoneUserReceive(IIdentityTools idRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<ChangePhoneUserRequestModel>?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ConfirmChangePhoneUserReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TAuthRequestStandardModel<ChangePhoneUserRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req?.Payload);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.GetType().Name, req, req.Payload.UserId);
        ResponseBaseModel res = await idRepo.ConfirmChangePhoneUserAsync(req, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res, res.GetType().Name), token);
        return res;
    }
}