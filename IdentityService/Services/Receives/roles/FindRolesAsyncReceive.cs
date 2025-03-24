////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Newtonsoft.Json;
using RemoteCallLib;
using SharedLib;

namespace Transmission.Receives.Identity;

/// <summary>
/// Роли. Если указан 'OwnerId', то поиск ограничивается ролями данного пользователя
/// </summary>
public class FindRolesAsyncReceive(IIdentityTools idRepo, ILogger<FindRolesAsyncReceive> loggerRepo)
    : IResponseReceive<FindWithOwnedRequestModel?, TPaginationResponseModel<RoleInfoModel>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstants.TransmissionQueues.FindRolesAsyncReceive;

    /// <summary>
    /// Роли. Если указан 'OwnerId', то поиск ограничивается ролями данного пользователя
    /// </summary>
    public async Task<TPaginationResponseModel<RoleInfoModel>?> ResponseHandleActionAsync(FindWithOwnedRequestModel? req, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(req);
        loggerRepo.LogWarning(JsonConvert.SerializeObject(req, GlobalStaticConstants.JsonSerializerSettings));
        return await idRepo.FindRoles(req, token);
    }
}