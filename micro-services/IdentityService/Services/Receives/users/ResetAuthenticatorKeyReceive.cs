////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Сбрасывает ключ аутентификации для пользователя.
/// </summary>
public class ResetAuthenticatorKeyReceive(IIdentityTools idRepo, ILogger<ResetAuthenticatorKeyReceive> loggerRepo)
    : IResponseReceive<string?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ResetAuthenticatorKeyReceive;

    /// <summary>
    /// Сбрасывает ключ аутентификации для пользователя.
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.ResetAuthenticatorKeyAsync(req, token);
    }
}