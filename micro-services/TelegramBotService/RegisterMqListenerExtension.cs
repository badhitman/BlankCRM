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
            .RegisterListenerRabbitMQ<SendTextMessageTelegramReceive, SendTextMessageTelegramBotModel, TResponseModel<MessageComplexIdsModel>>()
            .RegisterListenerRabbitMQ<SetWebConfigReceive, TelegramBotConfigModel, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<GetBotTokenReceive, object, TResponseModel<string>>()
            .RegisterListenerRabbitMQ<AboutBotReceive, object, TResponseModel<UserTelegramBaseModel>>()
            .RegisterListenerRabbitMQ<ChatsReadTelegramReceive, long[], List<ChatTelegramStandardModel>>()
            .RegisterListenerRabbitMQ<MessagesSelectTelegramReceive, TPaginationRequestStandardModel<SearchMessagesChatStandardModel>, TPaginationResponseStandardModel<MessageTelegramStandardModel>>()
            .RegisterListenerRabbitMQ<GetFileTelegramReceive, string, TResponseModel<byte[]>>()
            .RegisterListenerRabbitMQ<SendWappiMessageReceive, EntryAltExtModel, TResponseModel<SendMessageResponseModel?>>()
            .RegisterListenerRabbitMQ<ChatsFindForUserTelegramReceive, long[], List<ChatTelegramStandardModel>>()
            .RegisterListenerRabbitMQ<ChatsSelectTelegramReceive, TPaginationRequestStandardModel<string?>, TPaginationResponseStandardModel<ChatTelegramStandardModel>>()
            .RegisterListenerRabbitMQ<ForwardMessageTelegramReceive, ForwardMessageTelegramBotModel, TResponseModel<MessageComplexIdsModel>>()
            .RegisterListenerRabbitMQ<ChatTelegramReadReceive, int, ChatTelegramStandardModel>()
            .RegisterListenerRabbitMQ<ErrorsForChatsSelectTelegramReceive, TPaginationRequestStandardModel<long[]>, TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>>()
            ;
    }
}