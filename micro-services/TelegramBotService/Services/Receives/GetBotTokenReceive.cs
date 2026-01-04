////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.Extensions.Options;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Get TelegramBot Token
/// </summary>
public class GetBotTokenReceive(IOptions<BotConfiguration> tgConfig) : IResponseReceive<object?, TResponseModel<string>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetBotTokenTelegramReceive;

    /// <inheritdoc/>
    public async Task<TResponseModel<string>?> ResponseHandleActionAsync(object? req, CancellationToken token = default)
    {
#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
        return new TResponseModel<string>() { Response = tgConfig.Value.BotToken };
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
    }
}