////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Threading.Tasks;
using System.Threading;

namespace SharedLib;

/// <summary>
/// TelegramBot
/// </summary>
public interface ITelegramBotStandardService
{
    /// <summary>
    /// AboutBotAsync
    /// </summary>
    public Task<TResponseModel<UserTelegramBaseModel>> AboutBotAsync(CancellationToken token = default);


    /// <summary>
    /// SendTextMessageTelegram
    /// </summary>
    public Task<TResponseModel<MessageComplexIdsModel>> SendTextMessageTelegramAsync(SendTextMessageTelegramBotModel req, CancellationToken token = default);
}