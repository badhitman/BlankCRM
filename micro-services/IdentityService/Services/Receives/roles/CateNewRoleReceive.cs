////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Создать новую роль
/// </summary>
public class CateNewRoleReceive(IIdentityTools idRepo)
    : IResponseReceive<string?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.CateNewRoleReceive;

    /// <summary>
    /// Создать новую роль
    /// </summary>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(string? req, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(req))
            throw new ArgumentNullException(nameof(req));

        return await idRepo.CreateNewRoleAsync(req, token);
    }
}