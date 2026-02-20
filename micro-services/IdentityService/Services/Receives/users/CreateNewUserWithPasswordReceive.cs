////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Регистрация нового пользователя с паролем (Identity)
/// </summary>
public class CreateNewUserWithPasswordReceive(IIdentityTools idRepo)
    : IResponseReceive<RegisterNewUserPasswordModel?, RegistrationNewUserResponseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.RegistrationNewUserWithPasswordReceive;

    /// <inheritdoc/>
    public async Task<RegistrationNewUserResponseModel?> ResponseHandleActionAsync(RegisterNewUserPasswordModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        RegistrationNewUserResponseModel res = await idRepo.CreateNewUserWithPasswordAsync(req, token);
        return res;
    }
}