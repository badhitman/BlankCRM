﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить чаты
/// </summary>
public class ChatsSelectTelegramReceive(ITelegramBotService tgRepo) 
    : IResponseReceive<TPaginationRequestStandardModel<string?>?, TPaginationResponseModel<ChatTelegramModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ChatsSelectTelegramReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ChatTelegramModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<string?>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await tgRepo.ChatsSelectTelegramAsync(req, token);
    }
}