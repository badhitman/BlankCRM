////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// TelegramJoinAccountConfirm receive
/// </summary>
public class TelegramJoinAccountConfirmReceive(IIdentityTools identityRepo)
    : IResponseReceive<TelegramJoinAccountConfirmModel?, ResponseBaseModel?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.TelegramJoinAccountConfirmReceive;

    /// <inheritdoc/>
    public async Task<ResponseBaseModel?> ResponseHandleActionAsync(TelegramJoinAccountConfirmModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await identityRepo.TelegramJoinAccountConfirmTokenFromTelegramAsync(req, token: token);
    }
}