////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace IdentityService.Services.Receives.users;

/// <summary>
/// SelectUsersOfIdentityReceive
/// </summary>
public class SelectUsersOfIdentityReceive(IIdentityTools identityRepo)
    : IResponseReceive<TPaginationRequestModel<SimpleBaseRequestModel>?, TPaginationResponseModel<UserInfoModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.SelectUsersOfIdentityReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<UserInfoModel>?> ResponseHandleActionAsync(TPaginationRequestModel<SimpleBaseRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await identityRepo.SelectUsersOfIdentityAsync(req, token);
    }
}