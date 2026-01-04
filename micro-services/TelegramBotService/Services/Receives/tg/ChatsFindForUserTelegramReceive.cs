////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Найти чаты пользователей
/// </summary>
public class ChatsFindForUserTelegramReceive(ITelegramBotService tgRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<long[]?, List<ChatTelegramModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ChatsFindForUserTelegramReceive;

    /// <inheritdoc/>
    public async Task<List<ChatTelegramModelDB>?> ResponseHandleActionAsync(long[]? chats_ids, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(chats_ids);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, chats_ids.GetType().Name, JsonConvert.SerializeObject(chats_ids));
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await tgRepo.ChatsFindForUserTelegramAsync(chats_ids, token);
    }
}