////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// TelegramBotStandardTransmission
/// </summary>
public partial class TelegramBotStandardTransmission(IMQTTClient mqClient) : ITelegramBotStandardTransmission
{
    /// <inheritdoc/>
    public async Task<TResponseModel<UserTelegramBaseModel>> AboutBotAsync(CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<UserTelegramBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBotUsernameReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendTextMessageTelegramReceive, req, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ChatTelegramViewModel>> ChatsFindForUserTelegramAsync(long[] req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<List<ChatTelegramViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsFindForUserTelegramReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<List<ChatTelegramViewModel>> ChatsReadTelegramAsync(long[] req, CancellationToken token = default)
      => await mqClient.MqRemoteCallAsync<List<ChatTelegramViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsReadTelegramReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<ChatTelegramViewModel>> ChatsSelectTelegramAsync(TPaginationRequestStandardModel<string?> req, CancellationToken token = default)
      => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<ChatTelegramViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ChatTelegramViewModel> ChatTelegramReadAsync(int chatId, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<ChatTelegramViewModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatReadTelegramReceive, chatId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ForwardTextMessageTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<byte[]>> GetFileTelegramAsync(string req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<TResponseModel<byte[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<MessageTelegramViewModel>> MessagesSelectTelegramAsync(TPaginationRequestStandardModel<SearchMessagesChatModel> req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<TPaginationResponseModel<MessageTelegramViewModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessagesChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UserTelegramPermissionUpdateAsync(UserTelegramPermissionSetModel req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UserTelegramPermissionUpdateReceive, req, token: token) ?? new();
}