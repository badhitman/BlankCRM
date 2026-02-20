////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.DependencyInjection;
using SharedLib;

namespace RemoteCallLib;

/// <summary>
/// Удалённый вызов команд в Web службе
/// </summary>
public class WebTransmission([FromKeyedServices(nameof(RabbitClient))] IMQStandardClientRPC rabbitClient) : IWebTransmission
{
    /// <inheritdoc/>
    public async Task<TelegramBotConfigModel> GetWebConfigAsync(CancellationToken token = default)
        => await rabbitClient.MqRemoteCallAsync<TelegramBotConfigModel>(SharedLib.GlobalStaticConstantsTransmission.TransmissionQueues.GetWebConfigReceive, token: token) ?? new();
}