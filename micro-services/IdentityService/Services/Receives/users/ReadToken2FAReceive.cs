////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Чтение 2fa токена (из кеша)
/// </summary>
public class ReadToken2FAReceive(IIdentityTools idRepo)
    : IResponseReceive<string?, TResponseModel<string?>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.ReadToken2FAReceive;

    /// <summary>
    /// Чтение 2fa токена (из кеша)
    /// </summary>
    public async Task<TResponseModel<string?>?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.ReadToken2FAAsync(req, token);
    }
}