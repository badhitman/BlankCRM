////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.storage;

/// <summary>
/// Set web config site
/// </summary>
public class SetWebConfigReceive(WebConfigModel webConfig, ILogger<SetWebConfigReceive> _logger)
    : IResponseReceive<WebConfigModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SetWebConfigStorageReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(WebConfigModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(req)}");

#pragma warning disable CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
        if (!Uri.TryCreate(req.BaseUri, UriKind.Absolute, out _))
            return ResponseBaseModel.CreateError("BaseUri is null");

        return webConfig.Update(req.BaseUri);
#pragma warning restore CS8619 // Допустимость значения NULL для ссылочных типов в значении не соответствует целевому типу.
    }
}