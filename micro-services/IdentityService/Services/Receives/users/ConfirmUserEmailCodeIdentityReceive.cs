////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Проверяет, соответствует ли токен подтверждения электронной почты указанному пользователю.
/// </summary>
public class ConfirmUserEmailCodeIdentityReceive(IIdentityTools IdentityRepo, ILogger<ConfirmUserEmailCodeIdentityReceive> loggerRepo) 
    : IResponseReceive<UserCodeModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ConfirmUserEmailCodeIdentityReceive;

    /// <summary>
    /// Проверяет, соответствует ли токен подтверждения электронной почты указанному пользователю.
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(UserCodeModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await IdentityRepo.ConfirmEmailAsync(req, token);
    }
}