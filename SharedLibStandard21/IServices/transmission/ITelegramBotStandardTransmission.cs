////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharedLib;

/// <summary>
/// ITelegramBotStandardTransmission
/// </summary>
public interface ITelegramBotStandardTransmission : ITelegramBotStandardService
{
    /// <summary>
    /// ChatsFindForUserTelegram
    /// </summary>
    public Task<List<ChatTelegramViewModel>> ChatsFindForUserTelegramAsync(long[] req, CancellationToken token = default);

    /// <summary>
    /// ChatsReadTelegram
    /// </summary>
    public Task<List<ChatTelegramViewModel>> ChatsReadTelegramAsync(long[] req, CancellationToken token = default);

    /// <summary>
    /// ChatsSelectTelegram
    /// </summary>
    public Task<TPaginationResponseModel<ChatTelegramViewModel>> ChatsSelectTelegramAsync(TPaginationRequestStandardModel<string?> req, CancellationToken token = default);

    /// <summary>
    /// ChatTelegramRead
    /// </summary>
    public Task<ChatTelegramViewModel> ChatTelegramReadAsync(int chatId, CancellationToken token = default);

    /// <summary>
    /// ForwardMessageTelegram
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> ForwardMessageTelegramAsync(ForwardMessageTelegramBotModel req, CancellationToken token = default);

    /// <summary>
    /// GetFileTelegram
    /// </summary>
    public Task<TResponseModel<byte[]>> GetFileTelegramAsync(string req, CancellationToken token = default);

    /// <summary>
    /// MessagesSelectTelegram
    /// </summary>
    public Task<TPaginationResponseModel<MessageTelegramViewModel>> MessagesSelectTelegramAsync(TPaginationRequestStandardModel<SearchMessagesChatModel> req, CancellationToken token = default);
}
