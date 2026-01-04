////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Прочитать чаты
/// </summary>
public class ChatsReadTelegramReceive(ITelegramBotService tgRepo, IFilesIndexing indexingRepo)
    : IResponseReceive<long[]?, List<ChatTelegramModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ChatsReadTelegramReceive;

    /// <inheritdoc/>
    public async Task<List<ChatTelegramModelDB>?> ResponseHandleActionAsync(long[]? chats_ids, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(chats_ids);

        TraceReceiverRecord trace = TraceReceiverRecord.Build(GetType().Name, chats_ids.GetType().Name, JsonConvert.SerializeObject(chats_ids));
        await indexingRepo.SaveTraceForReceiverAsync(trace, token);

        return await tgRepo.ChatsReadTelegramAsync(chats_ids, token);
    }
}