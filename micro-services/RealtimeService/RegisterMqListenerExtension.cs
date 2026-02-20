////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Transmission.Receives.realtime;
using SharedLib;

namespace RealtimeService;

/// <summary>
/// MQ listen
/// </summary>
public static class RegisterMqListenerExtension
{
    /// <summary>
    /// RegisterMqListeners
    /// </summary>
    public static IServiceCollection RealtimeRegisterListenersRabbitMQ(this IServiceCollection services)
    {
        return services
            .RegisterListenerRabbitMQ<CreateMessageWebChatReceive, MessageWebChatModelDB, TResponseModel<int>>()
            .RegisterListenerRabbitMQ<UserInjectDialogWebChatReceive, TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpdateMessageWebChatReceive, TAuthRequestStandardModel<MessageWebChatModelDB>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpdateDialogWebChatInitiatorReceive, TAuthRequestStandardModel<DialogWebChatBaseModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<UpdateDialogWebChatAdminReceive, TAuthRequestStandardModel<DialogWebChatBaseModel>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<DeleteToggleDialogWebChatReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SelectUsersJoinsDialogsWebChatsReceive, TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel>, TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>>()
            .RegisterListenerRabbitMQ<DeleteToggleMessageWebChatReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<DeleteUserJoinDialogWebChatReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterListenerRabbitMQ<SelectMessagesWebChatReceive, SelectMessagesForWebChatRequestModel, TResponseModel<SelectMessagesForWebChatResponseModel>>()
            .RegisterListenerRabbitMQ<DialogsWebChatsReadReceive, TAuthRequestStandardModel<int[]>, TResponseModel<List<DialogWebChatModelDB>>>()
            .RegisterListenerRabbitMQ<InitWebChatSessionReceive, InitWebChatSessionRequestModel, TResponseModel<DialogWebChatModelDB>>()
            .RegisterListenerRabbitMQ<SelectMessagesForRoomWebChatReceive, TPaginationRequestAuthModel<SelectMessagesForWebChatRoomRequestModel>, TPaginationResponseStandardModel<MessageWebChatModelDB>>()
            .RegisterListenerRabbitMQ<SelectDialogsWebChatsReceive, TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel>, TPaginationResponseStandardModel<DialogWebChatModelDB>>()
            .RegisterListenerRabbitMQ<GetClientsConnectionsReceive, GetClientsRequestModel, TResponseModel<List<MqttClientModel>>>()
            
            
            ;
    }
}