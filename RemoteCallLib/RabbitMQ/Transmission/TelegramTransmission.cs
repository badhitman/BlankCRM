////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд в TelegramBot службе
/// </summary>
public class TelegramTransmission(IMQClientRPC rabbitClient) : ITelegramTransmission
{
    /// <inheritdoc/>
    public async Task<List<ChatTelegramStandardModel>> ChatsFindForUserTelegramAsync(long[] usersTelegramIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ChatTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsFindForUserTelegramReceive, usersTelegramIds, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<List<ChatTelegramStandardModel>> ChatsReadTelegramAsync(long[] chats_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ChatTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsReadTelegramReceive, chats_ids, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ChatTelegramStandardModel>> ChatsSelectTelegramAsync(TPaginationRequestStandardModel<string?> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ChatTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ChatTelegramStandardModel> ChatTelegramReadAsync(int chatIdDb, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ChatTelegramStandardModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatReadTelegramReceive, chatIdDb, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestStandardModel<long[]> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ErrorsForChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel message, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ForwardTextMessageTelegramReceive, message, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<UserTelegramBaseModel>> AboutBotAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<UserTelegramBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBotUsernameReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<byte[]>> GetFileTelegramAsync(string fileId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<byte[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileTelegramReceive, fileId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GetTelegramBotTokenAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBotTokenTelegramReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<MessageTelegramStandardModel>> MessagesTelegramSelectAsync(TPaginationRequestStandardModel<SearchMessagesChatStandardModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseStandardModel<MessageTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessagesChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel message_telegram, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendTextMessageTelegramReceive, message_telegram, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SendMessageResponseModel>> SendWappiMessageAsync(EntryAltExtModel message, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SendMessageResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendWappiMessageReceive, message, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigHelpDeskAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigHelpDeskReceive, webConf, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigStorageAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigStorageReceive, webConf, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigTelegramAsync(TelegramBotConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigTelegramReceive, webConf, waitResponse, token) ?? new();
}