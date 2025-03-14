﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Прочитать данные чата
/// </summary>
public class ChatTelegramReadReceive(ITelegramBotService tgRepo)
    : IResponseReceive<int, ChatTelegramModelDB?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ChatReadTelegramReceive;

    /// <inheritdoc/>
    public async Task<ChatTelegramModelDB?> ResponseHandleAction(int chat_id)
    {
        return await tgRepo.ChatTelegramRead(chat_id);
    }
}