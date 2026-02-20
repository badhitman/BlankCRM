////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить ошибки отправок сообщений (для чатов)
/// </summary>
public class ErrorsForChatsSelectTelegramReceive(ITelegramBotService tgRepo)
    : IResponseReceive<TPaginationRequestStandardModel<long[]>?, TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ErrorsForChatsSelectTelegramReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<long[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await tgRepo.ErrorsForChatsSelectTelegramAsync(req, token);
    }
}