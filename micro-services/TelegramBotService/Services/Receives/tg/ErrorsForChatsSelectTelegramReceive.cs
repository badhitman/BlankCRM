////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить ошибки отправок сообщений (для чатов)
/// </summary>
public class ErrorsForChatsSelectTelegramReceive(ITelegramBotService tgRepo)
    : IResponseReceive<TPaginationRequestStandardModel<long[]>?, TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ErrorsForChatsSelectTelegramReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<long[]>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        return await tgRepo.ErrorsForChatsSelectTelegramAsync(req, token);
    }
}