////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace IdentityService.Services.Receives.users;

/// <summary>
/// Сбрасывает пароль на указанный
/// после проверки заданного сброса пароля.
/// </summary>
public class ResetPasswordReceive(IIdentityTools idRepo)
    : IResponseReceive<IdentityPasswordTokenModel?, ResponseBaseModel?>
{
    /// <summary>
    /// Сбрасывает пароль на указанный
    /// после проверки заданного сброса пароля.
    /// </summary>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ResetPasswordForUserReceive;

    /// <summary>
    /// Сбрасывает пароль на указанный
    /// после проверки заданного сброса пароля.
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IdentityPasswordTokenModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.ResetPasswordAsync(req, token);
    }
}