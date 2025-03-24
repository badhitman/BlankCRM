﻿////////////////////////////////////////////////
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
    public Task<TResponseModel<string>> GetTelegramBotToken(CancellationToken token = default);

    /// <summary>
    /// Прочитать данные чата
    /// </summary>
    public Task<ChatTelegramModelDB> ChatTelegramRead(int chatIdDb, CancellationToken token = default);

    /// <summary>
    /// Переслать сообщение пользователю через TelegramBot
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessage(ForwardMessageTelegramBotModel message, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Получить Username для TelegramBot
    /// </summary>
    public Task<TResponseModel<string>> GetBotUsername(CancellationToken token = default);

    /// <summary>
    /// Отправить сообщение через Telegram бота
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegram(SendTextMessageTelegramBotModel message_telegram, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Отправить сообщение через Wappi
    /// </summary>
    public Task<TResponseModel<SendMessageResponseModel>> SendWappiMessage(EntryAltExtModel message, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// ChatsSelect
    /// </summary>
    public Task<TPaginationResponseModel<ChatTelegramModelDB>> ChatsSelect(TPaginationRequestModel<string?> req, CancellationToken token = default);

    /// <summary>
    /// Получить ошибки отправок сообщений (для чатов)
    /// </summary>
    public Task<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>> ErrorsForChatsSelectTelegram(TPaginationRequestModel<long[]?> req, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigTelegram(TelegramBotConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigHelpdesk(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Установить WebConfig. От web части отправляется значение при загрузке браузера
    /// </summary>
    public Task<ResponseBaseModel> SetWebConfigStorage(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные по чатам
    /// </summary>
    public Task<List<ChatTelegramModelDB>> ChatsReadTelegram(long[] chats_ids, CancellationToken token = default);

    /// <summary>
    /// Получить сообщения чата Telegram
    /// </summary>
    public Task<TPaginationResponseModel<MessageTelegramModelDB>> MessagesTelegramSelect(TPaginationRequestModel<SearchMessagesChatModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить данные файла
    /// </summary>
    public Task<TResponseModel<byte[]>> GetFile(string fileId, CancellationToken token = default);

    /// <summary>
    /// Найти связанные с пользователем чаты: чаты, в которых они засветились перед ботом
    /// </summary>
    /// <remarks>
    /// Для того что бы бот узнал о том что человек в каком-то чате нужно добавить бота в этот чат и написать любое сообщение в этот же чат.
    /// Таким образом при сохранении сообщений бот регистрирует факт связи чата с пользователем.
    /// </remarks>
    public Task<List<ChatTelegramModelDB>> ChatsFindForUser(long[] usersTelegramIds, CancellationToken token = default);
}