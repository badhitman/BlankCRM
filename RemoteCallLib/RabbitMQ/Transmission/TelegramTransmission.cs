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
    public async Task<List<ChatTelegramModelDB>> ChatsFindForUserAsync(long[] usersTelegramIds, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ChatTelegramModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsFindForUserTelegramReceive, usersTelegramIds, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<List<ChatTelegramModelDB>> ChatsReadTelegramAsync(long[] chats_ids, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<List<ChatTelegramModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsReadTelegramReceive, chats_ids, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ChatTelegramModelDB>> ChatsSelectAsync(TPaginationRequestModel<string?> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ChatTelegramModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ChatTelegramModelDB> ChatTelegramReadAsync(int chatIdDb, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ChatTelegramModelDB>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatReadTelegramReceive, chatIdDb, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestModel<long[]?> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<ErrorSendingMessageTelegramBotModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.ErrorsForChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageAsync(ForwardMessageTelegramBotModel message, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ForwardTextMessageTelegramReceive, message, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GetBotUsernameAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBotUsernameReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<byte[]>> GetFileAsync(string fileId, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<byte[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileTelegramReceive, fileId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<string>> GetTelegramBotTokenAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<string>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBotTokenTelegramReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<MessageTelegramModelDB>> MessagesTelegramSelectAsync(TPaginationRequestModel<SearchMessagesChatModel> req, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TPaginationResponseModel<MessageTelegramModelDB>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessagesChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel message_telegram, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendTextMessageTelegramReceive, message_telegram, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<SendMessageResponseModel>> SendWappiMessageAsync(EntryAltExtModel message, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TResponseModel<SendMessageResponseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendWappiMessageReceive, message, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigHelpdeskAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigHelpdeskReceive, webConf, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigStorageAsync(WebConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigStorageReceive, webConf, waitResponse, token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> SetWebConfigTelegramAsync(TelegramBotConfigModel webConf, bool waitResponse = true, CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigTelegramReceive, webConf, waitResponse, token) ?? new();
}