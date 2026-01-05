////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Пользователи
/// </summary>
public class FindUsersReceive(IIdentityTools idRepo)
    : IResponseReceive<FindWithOwnedRequestModel?, TPaginationResponseStandardModel<UserInfoModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FindUsersReceive;

    /// <summary>
    /// Пользователи
    /// </summary>
    public async Task<TPaginationResponseStandardModel<UserInfoModel>?> ResponseHandleActionAsync(FindWithOwnedRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.FindUsersAsync(req, token);
    }
}