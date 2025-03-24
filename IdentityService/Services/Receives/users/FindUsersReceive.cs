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
    : IResponseReceive<FindWithOwnedRequestModel?, TPaginationResponseModel<UserInfoModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.FindUsersReceive;

    /// <summary>
    /// Пользователи
    /// </summary>
    public async Task<TPaginationResponseModel<UserInfoModel>?> ResponseHandleActionAsync(FindWithOwnedRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.FindUsers(req, token);
    }
}