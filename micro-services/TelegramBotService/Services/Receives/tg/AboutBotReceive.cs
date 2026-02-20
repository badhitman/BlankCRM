////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Получить Username TelegramBot
/// </summary>
public class AboutBotReceive(ITelegramBotService tgRepo)
    : IResponseReceive<object?, TResponseModel<UserTelegramBaseModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetBotUsernameReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<UserTelegramBaseModel>?> ResponseHandleActionAsync(object? payload = null, CancellationToken token = default)
    {       
        return await tgRepo.AboutBotAsync(token);
    }
}