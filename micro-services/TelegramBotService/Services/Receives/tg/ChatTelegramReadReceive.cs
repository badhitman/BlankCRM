////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Прочитать данные чата
/// </summary>
public class ChatTelegramReadReceive(ITelegramBotService tgRepo)
    : IResponseReceive<int, ChatTelegramStandardModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ChatReadTelegramReceive;

    /// <inheritdoc/>
    public async Task<ChatTelegramStandardModel?> ResponseHandleActionAsync(int chat_id, CancellationToken token = default)
    {
        if (chat_id <= 0)
            throw new Exception($"chat id incorrect: {chat_id}");

        return await tgRepo.ChatTelegramReadAsync(chat_id, token);
    }
}