////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Transmission.Receives.telegram;
using SharedLib;

namespace TelegramBotService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection TelegramBotRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<SendTextMessageTelegramReceive, SendTextMessageTelegramBotModel, TResponseModel<MessageComplexIdsModel>>()
            .RegisterMqListener<SetWebConfigReceive, TelegramBotConfigModel, ResponseBaseModel>()
            .RegisterMqListener<GetBotTokenReceive, object, TResponseModel<string>>()
            .RegisterMqListener<AboutBotReceive, object, TResponseModel<UserTelegramBaseModel>>()
            .RegisterMqListener<ChatsReadTelegramReceive, long[], List<ChatTelegramModelDB>>()
            .RegisterMqListener<MessagesSelectTelegramReceive, TPaginationRequestStandardModel<SearchMessagesChatModel>, TPaginationResponseModel<MessageTelegramModelDB>>()
            .RegisterMqListener<GetFileTelegramReceive, string, TResponseModel<byte[]>>()
            .RegisterMqListener<SendWappiMessageReceive, EntryAltExtModel, TResponseModel<SendMessageResponseModel?>>()
            .RegisterMqListener<ChatsFindForUserTelegramReceive, long[], List<ChatTelegramModelDB>>()
            .RegisterMqListener<ChatsSelectTelegramReceive, TPaginationRequestStandardModel<string?>, TPaginationResponseModel<ChatTelegramModelDB>>()
            .RegisterMqListener<ForwardMessageTelegramReceive, ForwardMessageTelegramBotModel, TResponseModel<MessageComplexIdsModel>>()
            .RegisterMqListener<ChatTelegramReadReceive, int, ChatTelegramModelDB>()
            .RegisterMqListener<ErrorsForChatsSelectTelegramReceive, TPaginationRequestStandardModel<long[]>, TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>>()
            ;
    }
}