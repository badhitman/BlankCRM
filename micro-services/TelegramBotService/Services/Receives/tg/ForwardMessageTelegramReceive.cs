////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Переслать сообщение пользователю через TelegramBot ForwardMessageTelegramReceive
/// </summary>
public class ForwardMessageTelegramReceive(ITelegramBotService tgRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<ForwardMessageTelegramBotModel?, TResponseModel<MessageComplexIdsModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ForwardTextMessageTelegramReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>?> ResponseHandleActionAsync(ForwardMessageTelegramBotModel? message, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        TraceReceiverRecord trace = TraceReceiverRecord.Build(QueueName, message.GetType().Name, message, _requestKey: message.SourceMessageId.ToString());
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);
        TResponseModel<MessageComplexIdsModel> res = await tgRepo.ForwardMessageTelegramAsync(message, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);
        return res;
    }
}