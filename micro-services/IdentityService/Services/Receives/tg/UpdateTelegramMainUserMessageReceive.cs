////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Update Telegram main user message
/// </summary>
public class UpdateTelegramMainUserMessageReceive(IIdentityTools identityRepo, ILogger<UpdateTelegramMainUserMessageReceive> _logger) : IResponseReceive<MainUserMessageModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.UpdateTelegramMainUserMessageReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(MainUserMessageModel? setMainMessage, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(setMainMessage);
        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(setMainMessage, GlobalStaticConstants.JsonSerializerSettings)}");
        return await identityRepo.UpdateTelegramMainUserMessageAsync(setMainMessage, token);
    }
}