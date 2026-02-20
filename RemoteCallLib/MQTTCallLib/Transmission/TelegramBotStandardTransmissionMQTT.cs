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
public partial class TelegramBotStandardTransmissionMQTT(IMQClientExtRPC mqClient) : ITelegramBotStandardTransmission
{
    /// <inheritdoc/>
    public async Task<TResponseModel<UserTelegramBaseModel>> AboutBotAsync(CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<UserTelegramBaseModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.GetBotUsernameReceive, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.SendTextMessageTelegramReceive, req, waitResponse: false, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<UserTelegramStandardModel>> UsersReadTelegramAsync(int[] req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<List<UserTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.UsersReadTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<MessageTelegramStandardModel>> MessagesTelegramSelectAsync(TPaginationRequestStandardModel<SearchMessagesChatStandardModel> req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<TPaginationResponseStandardModel<MessageTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.MessagesChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ChatTelegramStandardModel>> ChatsReadTelegramAsync(long[] req, CancellationToken token = default)
      => await mqClient.MqRemoteCallAsync<List<ChatTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsReadTelegramReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<ChatTelegramStandardModel> ChatTelegramReadAsync(int chatId, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<ChatTelegramStandardModel>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatReadTelegramReceive, chatId, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ChatTelegramStandardModel>> ChatsSelectTelegramAsync(TPaginationRequestStandardModel<string?> req, CancellationToken token = default)
      => await mqClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ChatTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<List<ChatTelegramStandardModel>> ChatsFindForUserTelegramAsync(long[] req, CancellationToken token = default)
        => await mqClient.MqRemoteCallAsync<List<ChatTelegramStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ChatsFindForUserTelegramReceive, req, token: token) ?? [];

    /// <inheritdoc/>
    public async Task<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>> ErrorsForChatsSelectTelegramAsync(TPaginationRequestStandardModel<long[]> req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<TPaginationResponseStandardModel<ErrorSendingMessageTelegramBotStandardModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ErrorsForChatsSelectTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<TResponseModel<MessageComplexIdsModel>>(GlobalStaticConstantsTransmission.TransmissionQueues.ForwardTextMessageTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<TResponseModel<byte[]>> GetFileTelegramAsync(string req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<TResponseModel<byte[]>>(GlobalStaticConstantsTransmission.TransmissionQueues.ReadFileTelegramReceive, req, token: token) ?? new();

    /// <inheritdoc/>
    public async Task<ResponseBaseModel> UserTelegramPermissionUpdateAsync(UserTelegramPermissionSetModel req, CancellationToken token = default)
       => await mqClient.MqRemoteCallAsync<ResponseBaseModel>(GlobalStaticConstantsTransmission.TransmissionQueues.UserTelegramPermissionUpdateReceive, req, token: token) ?? new();
}