////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Проверяет указанную двухфакторную аутентификацию VerificationCode на соответствие UserId
/// </summary>
public class VerifyTwoFactorTokenReceive(IIdentityTools idRepo, ILogger<VerifyTwoFactorTokenReceive> loggerRepo)
    : IResponseReceive<VerifyTwoFactorTokenRequestModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.VerifyTwoFactorTokenReceive;

    /// <summary>
    /// Проверяет указанную двухфакторную аутентификацию VerificationCode на соответствие UserId
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(VerifyTwoFactorTokenRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.VerifyTwoFactorTokenAsync(req, token);
    }
}