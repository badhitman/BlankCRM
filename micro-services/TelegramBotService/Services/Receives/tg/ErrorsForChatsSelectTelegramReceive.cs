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
    : IResponseReceive<TPaginationRequestModel<long[]>?, TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ErrorsForChatsSelectTelegramReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>?> ResponseHandleActionAsync(TPaginationRequestModel<long[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await tgRepo.ErrorsForChatsSelectTelegramAsync(req, token);
    }
}