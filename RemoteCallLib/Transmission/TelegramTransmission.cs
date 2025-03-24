////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд в TelegramBot службе
/// </summary>
public class TelegramTransmission(IRabbitClient rabbitClient) : ITelegramTransmission
{
    /// <inheritdoc/>
    public async Task<List<ChatTelegramModelDB>> ChatsFindForUser(long[] usersTelegramIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<List<ChatTelegramModelDB>>(GlobalStaticConstants.TransmissionQueues.ChatsFindForUserTelegramReceive, usersTelegramIds, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<List<ChatTelegramModelDB>> ChatsReadTelegram(long[] chats_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<List<ChatTelegramModelDB>>(GlobalStaticConstants.TransmissionQueues.ChatsReadTelegramReceive, chats_ids, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ChatTelegramModelDB>> ChatsSelect(TPaginationRequestModel<string?> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TPaginationResponseModel<ChatTelegramModelDB>>(GlobalStaticConstants.TransmissionQueues.ChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ChatTelegramModelDB> ChatTelegramRead(int chatIdDb, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ChatTelegramModelDB>(GlobalStaticConstants.TransmissionQueues.ChatReadTelegramReceive, chatIdDb, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>> ErrorsForChatsSelectTelegram(TPaginationRequestModel<long[]?> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>>(GlobalStaticConstants.TransmissionQueues.ErrorsForChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> ForwardMessage(ForwardMessageTelegramBotModel message, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstants.TransmissionQueues.ForwardTextMessageTelegramReceive, message, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GetBotUsername(CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<string>>(GlobalStaticConstants.TransmissionQueues.GetBotUsernameReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<byte[]>> GetFile(string fileId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<byte[]>>(GlobalStaticConstants.TransmissionQueues.ReadFileTelegramReceive, fileId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GetTelegramBotToken(CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<string>>(GlobalStaticConstants.TransmissionQueues.GetBotTokenTelegramReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<MessageTelegramModelDB>> MessagesTelegramSelect(TPaginationRequestModel<SearchMessagesChatModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TPaginationResponseModel<MessageTelegramModelDB>>(GlobalStaticConstants.TransmissionQueues.MessagesChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegram(SendTextMessageTelegramBotModel message_telegram, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstants.TransmissionQueues.SendTextMessageTelegramReceive, message_telegram, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SendMessageResponseModel>> SendWappiMessage(EntryAltExtModel message, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<TResponseModel<SendMessageResponseModel>>(GlobalStaticConstants.TransmissionQueues.SendWappiMessageReceive, message, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigHelpdesk(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetWebConfigHelpdeskReceive, webConf, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigStorage(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetWebConfigStorageReceive, webConf, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigTelegram(TelegramBotConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCall<ResponseBaseModel>(GlobalStaticConstants.TransmissionQueues.SetWebConfigTelegramReceive, webConf, waitResponse, token) ?? new();
}