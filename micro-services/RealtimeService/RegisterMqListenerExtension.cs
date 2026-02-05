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
    public static IServiceCollection RealtimeRegisterMqListeners(this IServiceCollection services)
    {
        return services
            .RegisterMqListener<CreateMessageWebChatReceive, MessageWebChatModelDB, TResponseModel<int>>()
            .RegisterMqListener<UserInjectDialogWebChatReceive, TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel>, ResponseBaseModel>()
            .RegisterMqListener<UpdateMessageWebChatReceive, TAuthRequestStandardModel<MessageWebChatModelDB>, ResponseBaseModel>()
            .RegisterMqListener<UpdateDialogWebChatInitiatorReceive, TAuthRequestStandardModel<DialogWebChatBaseModel>, ResponseBaseModel>()
            .RegisterMqListener<UpdateDialogWebChatAdminReceive, TAuthRequestStandardModel<DialogWebChatBaseModel>, ResponseBaseModel>()
            .RegisterMqListener<DeleteToggleDialogWebChatReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<SelectUsersJoinsDialogsWebChatsReceive, TPaginationRequestStandardModel<SelectUsersJoinsDialogsWebChatsRequestModel>, TPaginationResponseStandardModel<UserJoinDialogWebChatModelDB>>()
            .RegisterMqListener<DeleteToggleMessageWebChatReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<DeleteUserJoinDialogWebChatReceive, TAuthRequestStandardModel<int>, ResponseBaseModel>()
            .RegisterMqListener<SelectMessagesWebChatReceive, SelectMessagesForWebChatRequestModel, TResponseModel<SelectMessagesForWebChatResponseModel>>()
            .RegisterMqListener<DialogsWebChatsReadReceive, TAuthRequestStandardModel<int[]>, TResponseModel<List<DialogWebChatModelDB>>>()
            .RegisterMqListener<InitWebChatSessionReceive, InitWebChatSessionRequestModel, TResponseModel<DialogWebChatModelDB>>()
            .RegisterMqListener<SelectMessagesForRoomWebChatReceive, TPaginationRequestAuthModel<SelectMessagesForWebChatRoomRequestModel>, TPaginationResponseStandardModel<MessageWebChatModelDB>>()
            .RegisterMqListener<SelectDialogsWebChatsReceive, TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel>, TPaginationResponseStandardModel<DialogWebChatModelDB>>()
            ;
    }
}