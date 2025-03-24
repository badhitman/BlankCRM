////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TelegramBot
/// </summary>
public interface ITelegramBotService
{
    /// <summary>
    /// ChatsFindForUserTelegram
    /// </summary>
    public Task<List<ChatTelegramModelDB>> ChatsFindForUserTelegramAsync(long[] req, CancellationToken token);

    /// <summary>
    /// ChatsReadTelegram
    /// </summary>
    public Task<List<ChatTelegramModelDB>> ChatsReadTelegramAsync(long[] req, CancellationToken token);

    /// <summary>
    /// ChatsSelectTelegram
    /// </summary>
    public Task<TPaginationResponseModel<ChatTelegramModelDB>> ChatsSelectTelegramAsync(TPaginationRequestModel<string?> req, CancellationToken token);

    /// <summary>
    /// ChatTelegramRead
    /// </summary>
    public Task<ChatTelegramModelDB> ChatTelegramReadAsync(int chatId, CancellationToken token);

    /// <summary>
    /// ErrorsForChatsSelectTelegram
    /// </summary>
    public Task<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestModel<long[]> req, CancellationToken token);

    /// <summary>
    /// ForwardMessageTelegram
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel req, CancellationToken token);

    /// <summary>
    /// GetBotUsername
    /// </summary>
    public Task<TResponseModel<string>> GetBotUsernameAsync(CancellationToken token);

    /// <summary>
    /// GetFileTelegram
    /// </summary>
    public Task<TResponseModel<byte[]>> GetFileTelegramAsync(string req, CancellationToken token);

    /// <summary>
    /// SendTextMessageTelegram
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel req, CancellationToken token);

    /// <summary>
    /// MessagesSelectTelegram
    /// </summary>
    public Task<TPaginationResponseModel<MessageTelegramModelDB>> MessagesSelectTelegramAsync(TPaginationRequestModel<SearchMessagesChatModel> req, CancellationToken token);
}
