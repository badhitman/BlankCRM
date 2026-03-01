////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Роли. Если указан 'OwnerId', то поиск ограничивается ролями данного пользователя
/// </summary>
public class FindRolesAsyncReceive(IIdentityTools idRepo)
    : IResponseReceive<FindWithOwnedRequestModel?, TPaginationResponseStandardModel<RoleInfoModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.FindRolesAsyncReceive;

    /// <summary>
    /// Роли. Если указан 'OwnerId', то поиск ограничивается ролями данного пользователя
    /// </summary>
    public async Task<TPaginationResponseStandardModel<RoleInfoModel>?> ResponseHandleActionAsync(FindWithOwnedRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        return await idRepo.FindRolesAsync(req, token);
    }
}