////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// TelegramBot
/// </summary>
public interface ITelegramBotStandardService
{
    /// <summary>
    /// Получить сообщения чата Telegram
    /// </summary>
    public Task<TPaginationResponseStandardModel<MessageTelegramStandardModel>> MessagesTelegramSelectAsync(TPaginationRequestStandardModel<SearchMessagesChatStandardModel> req, CancellationToken token = default);

    /// <summary>
    /// Получить данные файла
    /// </summary>
    public Task<TResponseModel<byte[]>> GetFileTelegramAsync(string fileId, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные по чатам
    /// </summary>
    public Task<List<ChatTelegramStandardModel>> ChatsReadTelegramAsync(long[] chats_ids, CancellationToken token = default);

    /// <summary>
    /// Найти связанные с пользователем чаты: чаты, в которых они засветились перед ботом
    /// </summary>
    /// <remarks>
    /// Для того что бы бот узнал о том что человек в каком-то чате нужно добавить бота в этот чат и написать любое сообщение в этот же чат.
    /// Таким образом при сохранении сообщений бот регистрирует факт связи чата с пользователем.
    /// </remarks>
    public Task<List<ChatTelegramStandardModel>> ChatsFindForUserTelegramAsync(long[] usersTelegramIds, CancellationToken token = default);

    /// <summary>
    /// Прочитать данные чата
    /// </summary>
    public Task<ChatTelegramStandardModel> ChatTelegramReadAsync(int chatIdDb, CancellationToken token = default);

    /// <summary>
    /// ChatsSelect
    /// </summary>
    public Task<TPaginationResponseStandardModel<ChatTelegramStandardModel>> ChatsSelectTelegramAsync(TPaginationRequestStandardModel<string?> req, CancellationToken token = default);

    /// <summary>
    /// Получить Username для TelegramBot
    /// </summary>
    public Task<TResponseModel<UserTelegramBaseModel>> AboutBotAsync(CancellationToken token = default);

    /// <summary>
    /// Получить ошибки отправок сообщений (для чатов)
    /// </summary>
    public Task<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestStandardModel<long[]> req, CancellationToken token = default);
}