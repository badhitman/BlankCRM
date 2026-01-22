////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

namespace SharedLib;

/// <summary>
/// TelegramBot
/// </summary>
public interface ITelegramBotService : ITelegramBotStandardService
{
    /// <summary>
    /// SendTextMessageTelegram
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel req, CancellationToken token = default);

    /// <summary>
    /// ForwardMessageTelegram
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel req, CancellationToken token = default);

    /// <summary>
    /// ErrorsForChatsSelectTelegram
    /// </summary>
    public Task<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotModelDB>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestStandardModel<long[]> req, CancellationToken token = default);

    /// <summary>
    /// MessagesSelectTelegram
    /// </summary>
    public Task<TPaginationResponseStandardModel<MessageTelegramModelDB>> MessagesSelectTelegramAsync(TPaginationRequestStandardModel<SearchMessagesChatModel> req, CancellationToken token = default);
}