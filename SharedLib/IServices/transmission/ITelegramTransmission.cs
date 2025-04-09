////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// Удалённый вызов команд в TelegramBot службе
/// </summary>
public interface ITelegramTransmission
{
    /// <summary>
    /// Получить токен TG бота (для расчёта HMAC хеша)
    /// </summary>
    public Task<TResponseModel<string>> GetTelegramBotTokenAsync(CancellationToken token = default);

    /// <summary>
    /// Прочитать данные чата
    /// </summary>
    public Task<ChatTelegramModelDB> ChatTelegramReadAsync(int chatIdDb, CancellationToken token = default);

    /// <summary>
    /// Переслать сообщение пользователю через TelegramBot
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageAsync(ForwardMessageTelegramBotModel message, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Получить Username для TelegramBot
    /// </summary>
    public Task<TResponseModel<string>> GetBotUsernameAsync(CancellationToken token = default);

    /// <summary>
    /// Отправить сообщение через Telegram бота
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel message_telegram, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Отправить сообщение через Wappi
    /// </summary>
    public Task<TResponseModel<SendMessageResponseModel>> SendWappiMessageAsync(EntryAltExtModel message, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// ChatsSelect
    /// </summary>
    public Task<TPaginationResponseModel<ChatTelegramModelDB>> ChatsSelectAsync(TPaginationRequestModel<string?> req, CancellationToken token = default);

    /// <summary>
    /// Получить ошибки отправок сообщений (для чатов)
    /// </summary>
    public Task<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestModel<long[]?> req, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigTelegramAsync(TelegramBotConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigHelpdeskAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigStorageAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные по чатам
    /// </summary>
    public Task<List<ChatTelegramModelDB>> ChatsReadTelegramAsync(long[] chats_ids, CancellationToken token = default);

    /// <summary>
    /// Получить сообщения чата Telegram
    /// </summary>
    public Task<TPaginationResponseModel<MessageTelegramModelDB>> MessagesTelegramSelectAsync(TPaginationRequestModel<SearchMessagesChatModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить данные файла
    /// </summary>
    public Task<TResponseModel<byte[]>> GetFileAsync(string fileId, CancellationToken token = default);

    /// <summary>
    /// Найти связанные с пользователем чаты: чаты, в которых они засветились перед ботом
    /// </summary>
    /// <remarks>
    /// Для того что бы бот узнал о том что человек в каком-то чате нужно добавить бота в этот чат и написать любое сообщение в этот же чат.
    /// Таким образом при сохранении сообщений бот регистрирует факт связи чата с пользователем.
    /// </remarks>
    public Task<List<ChatTelegramModelDB>> ChatsFindForUserAsync(long[] usersTelegramIds, CancellationToken token = default);
}