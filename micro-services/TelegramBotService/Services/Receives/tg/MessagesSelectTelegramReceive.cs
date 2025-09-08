////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить сообщения из чата
/// </summary>
public class MessagesSelectTelegramReceive(ITelegramBotService tgRepo)
    : IResponseReceive<TPaginationRequestStandardModel<SearchMessagesChatModel>?, TPaginationResponseModel<MessageTelegramModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MessagesChatsSelectTelegramReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<MessageTelegramModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SearchMessagesChatModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await tgRepo.MessagesSelectTelegramAsync(req, token);
    }
}