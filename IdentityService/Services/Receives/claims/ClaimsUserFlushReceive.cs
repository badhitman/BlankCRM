////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Установить пользователю Claim`s[TelegramId, FirstName, LastName, PhoneNum]
/// </summary>
public class ClaimsUserFlushReceive(IIdentityTools idRepo)
    : IResponseReceive<string?, TResponseModel<bool>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.ClaimsForUserFlushReceive;

    /// <summary>
    /// Установить пользователю Claim`s[TelegramId, FirstName, LastName, PhoneNum]
    /// </summary>
    public async Task<TResponseModel<bool>?> ResponseHandleAction(string? userId, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentNullException(nameof(userId));

        return await idRepo.ClaimsUserFlush(userId, token);
    }
}