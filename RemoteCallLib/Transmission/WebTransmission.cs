////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд в Web службе
/// </summary>
public class WebTransmission(IRabbitClient rabbitClient) : IWebTransmission
{
    /// <inheritdoc/>
    public async Task<TelegramBotConfigModel> GetWebConfigAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TelegramBotConfigModel>(GlobalStaticConstants.TransmissionQueues.GetWebConfigReceive, token: token) ?? new();
}