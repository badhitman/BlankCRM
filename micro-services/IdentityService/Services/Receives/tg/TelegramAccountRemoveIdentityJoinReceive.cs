﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Удалить связь Telegram аккаунта с учётной записью сайта
/// </summary>
public class TelegramAccountRemoveIdentityJoinReceive(IIdentityTools identityRepo, ILogger<TelegramAccountRemoveIdentityJoinReceive> _logger)
    : IResponseReceive<TelegramAccountRemoveJoinRequestIdentityModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TelegramAccountRemoveIdentityJoinReceive;

    /// <summary>
    /// Удалить связь Telegram аккаунта с учётной записью сайта
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TelegramAccountRemoveJoinRequestIdentityModel? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        _logger.LogInformation($"call `{GetType().Name}`: {JsonConvert.SerializeObject(payload, GlobalStaticConstants.JsonSerializerSettings)}");
        return await identityRepo.TelegramAccountRemoveIdentityJoinAsync(payload, token);
    }
}