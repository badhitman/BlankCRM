////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Найти чаты пользователей
/// </summary>
public class ChatsFindForUserTelegramReceive(ITelegramBotService tgRepo)
    : IResponseReceive<long[]?, List<ChatTelegramViewModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ChatsFindForUserTelegramReceive;

    /// <inheritdoc/>
    public async Task<List<ChatTelegramViewModel>?> ResponseHandleActionAsync(long[]? chats_ids, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(chats_ids);
        return await tgRepo.ChatsFindForUserTelegramAsync(chats_ids, token);
    }
}