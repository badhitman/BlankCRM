////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Ключ аутентификации пользователя.
/// </summary>
public class GetAuthenticatorKeyReceive(IIdentityTools idRepo)
    : IResponseReceive<string?, TResponseModel<string?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetAuthenticatorKeyReceive;

    /// <summary>
    /// Ключ аутентификации пользователя.
    /// </summary>
    public async Task<TResponseModel<string?>?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);        
        return await idRepo.GetAuthenticatorKeyAsync(req, token);
    }
}