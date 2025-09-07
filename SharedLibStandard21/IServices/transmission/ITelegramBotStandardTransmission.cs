////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// ITelegramBotStandardTransmission
/// </summary>
public interface ITelegramBotStandardTransmission : ITelegramBotStandardService
{
    /// <inheritdoc/>
    public Task<ResponseBaseModel> UserTelegramPermissionUpdateAsync(UserTelegramPermissionSetModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<List<ChatTelegramViewModel>> ChatsFindForUserTelegramAsync(long[] req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<List<ChatTelegramViewModel>> ChatsReadTelegramAsync(long[] req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<ChatTelegramViewModel>> ChatsSelectTelegramAsync(TPaginationRequestStandardModel<string?> req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<ChatTelegramViewModel> ChatTelegramReadAsync(int chatId, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TResponseModel<byte[]>> GetFileTelegramAsync(string req, CancellationToken token = default);

    /// <inheritdoc/>
    public Task<TPaginationResponseModel<MessageTelegramViewModel>> MessagesSelectTelegramAsync(TPaginationRequestStandardModel<SearchMessagesChatModel> req, CancellationToken token = default);
}