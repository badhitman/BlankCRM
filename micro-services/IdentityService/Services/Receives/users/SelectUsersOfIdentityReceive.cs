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
    : IResponseReceive<TPaginationRequestStandardModel<SimpleBaseRequestModel>?, TPaginationResponseModel<UserInfoModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.SelectUsersOfIdentityReceive;

    /// <inheritdoc/>
    public async Task<TPaginationResponseModel<UserInfoModel>?> ResponseHandleActionAsync(TPaginationRequestStandardModel<SimpleBaseRequestModel>? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await identityRepo.SelectUsersOfIdentityAsync(req, token);
    }
}