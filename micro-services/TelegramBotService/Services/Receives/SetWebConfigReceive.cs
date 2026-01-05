////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.telegram;

/// <summary>
/// Set web config site
/// </summary>
public class SetWebConfigReceive(TelegramBotConfigModel webConfig, ILogger<SetWebConfigReceive> _logger)
    : IResponseReceive<TelegramBotConfigModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigTelegramReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TelegramBotConfigModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings)}");

        ResponseBaseModel upd = webConfig.Update(req);

#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
        return ResponseBaseModel.Create(upd.Messages);
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
    }
}