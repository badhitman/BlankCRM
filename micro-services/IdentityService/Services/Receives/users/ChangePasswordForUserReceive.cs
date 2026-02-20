////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Изменяет пароль пользователя после подтверждения правильности указанного currentPassword
/// </summary>
public class ChangePasswordForUserReceive(IIdentityTools idRepo)
    : IResponseReceive<IdentityChangePasswordModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ChangePasswordToUserReceive;

    /// <summary>
    /// Изменяет пароль пользователя после подтверждения правильности указанного currentPassword
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(IdentityChangePasswordModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.ChangePasswordAsync(req, token);
    }
}