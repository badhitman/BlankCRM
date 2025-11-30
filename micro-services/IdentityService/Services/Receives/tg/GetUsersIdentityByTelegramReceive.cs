////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.web;

/// <summary>
/// Find user identity by telegram - receive
/// </summary>
public class GetUsersIdentityByTelegramReceive(IIdentityTools identityRepo)
    : IResponseReceive<long[]?, TResponseModel<UserInfoModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetUsersIdentityByTelegramReceive;

    /// <summary>
    /// Find user identity by telegram - receive
    /// </summary>
    public async Task<TResponseModel<UserInfoModel[]>?> ResponseHandleActionAsync(long[]? payload, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(payload);
        return await identityRepo.GetUsersIdentityByTelegramAsync(payload, token);
    }
}