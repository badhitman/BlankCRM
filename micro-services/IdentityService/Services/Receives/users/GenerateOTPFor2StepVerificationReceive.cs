////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Генерация (и отправка на Email++) 2fa токена
/// </summary>
public class GenerateOTPFor2StepVerificationReceive(IIdentityTools idRepo, ILogger<GenerateOTPFor2StepVerificationReceive> loggerRepo)
    : IResponseReceive<string?, TResponseModel<string>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GenerateToken2FAReceive;

    /// <summary>
    /// Генерация (и отправка на Email++) 2fa токена
    /// </summary>
    public async Task<TResponseModel<string>?> ResponseHandleActionAsync(string? userId, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(userId);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(userId, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.GenerateToken2FAAsync(userId, token);
    }
}