////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Включена ли для указанного userId двухфакторная аутентификация.
/// </summary>
public class GetTwoFactorEnabledReceive(IIdentityTools idRepo)
    : IResponseReceive<string?, TResponseModel<bool?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetTwoFactorEnabledReceive;

    /// <summary>
    /// Включена ли для указанного <paramref name="userId"/> двухфакторная аутентификация
    /// </summary>
    public async Task<TResponseModel<bool?>?> ResponseHandleActionAsync(string? userId, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(userId);        
        return await idRepo.GetTwoFactorEnabledAsync(userId, token);
    }
}