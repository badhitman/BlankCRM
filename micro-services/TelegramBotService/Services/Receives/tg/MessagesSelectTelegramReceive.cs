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
    : IResponseReceive<TPaginationRequestStandardModel<SearchMessagesChatStandardModel>?, TPaginationResponseStandardModel<MessageTelegramStandardModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.MessagesChatsSelectTelegramReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<MessageTelegramStandardModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SearchMessagesChatStandardModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await tgRepo.MessagesTelegramSelectAsync(req, token);
    }
}