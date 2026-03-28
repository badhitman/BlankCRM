////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.firebase;

/// <summary>
/// Get Firebase config
/// </summary>
public class SendFirebaseMessageReceive(IFirebaseService firebaseRepo, IHistoryIndexing indexingRepo)
    : IResponseReceive<TAuthRequestStandardModel<SendFirebaseMessageRequestModel>?, TResponseModel<SendFirebaseMessageResultModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SendFirebaseMessageReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<SendFirebaseMessageResultModel>?> ResponseHandleActionAsync(TAuthRequestStandardModel<SendFirebaseMessageRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, req.SenderActionUserId, req.Payload);
        TResponseModel<SendFirebaseMessageResultModel> res = await firebaseRepo.SendFirebaseMessageAsync(req, token);
        await indexingRepo.SaveHistoryForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}