////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Отправить сообщение пользователю через TelegramBot SendTextMessageTelegramBotModel
/// </summary>
public class SendTextMessageTelegramReceive(ITelegramBotService tgRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<SendTextMessageTelegramBotModel?, TResponseModel<MessageComplexIdsModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SendTextMessageTelegramReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>?> ResponseHandleActionAsync(SendTextMessageTelegramBotModel? message, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name);
        TResponseModel<MessageComplexIdsModel> res = await tgRepo.SendTextMessageTelegramAsync(message, token);
        await indexingRepo.SaveTraceForReceiverAsync(trace.SetResponse(res), token);

        return res;
    }
}