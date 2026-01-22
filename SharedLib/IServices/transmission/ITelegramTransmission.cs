////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Удалённый вызов команд в TelegramBot службе
/// </summary>
public interface ITelegramTransmission : ITelegramBotStandardService
{
    /// <summary>
    /// Отправить сообщение через Telegram бота
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel message_telegram, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Переслать сообщение пользователю через TelegramBot
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel message, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Отправить сообщение через Wappi
    /// </summary>
    public Task<TResponseModel<SendMessageResponseModel>> SendWappiMessageAsync(EntryAltExtModel message, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigTelegramAsync(TelegramBotConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigHelpDeskAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigStorageAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default);


    /// <summary>
    /// Получить токен TG бота (для расчёта HMAC хеша)
    /// </summary>
    public Task<TResponseModel<string>> GetTelegramBotTokenAsync(CancellationToken token = default);

    /// <summary>
    /// Получить ошибки отправок сообщений (для чатов)
    /// </summary>
    public Task<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotModelDB>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestStandardModel<long[]> req, CancellationToken token = default);

    /// <summary>
    /// Получить сообщения чата Telegram
    /// </summary>
    public Task<TPaginationResponseStandardModel<MessageTelegramModelDB>> MessagesTelegramSelectAsync(TPaginationRequestStandardModel<SearchMessagesChatModel> req, CancellationToken token = default);
}