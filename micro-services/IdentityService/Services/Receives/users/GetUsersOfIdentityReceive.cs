////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.Extensions.Caching.Memory;
using RemoteCallLib;
using IdentityLib;
using SharedLib;

namespace IdentityService.Services.Receives.users;

/// <summary>
/// Получить пользователей из Identity по их идентификаторам
/// </summary>
public class GetUsersOfIdentityReceive(IIdentityTools identityRepo)
    : IResponseReceive<string[]?, TResponseModel<UserInfoModel[]>?>
{
    /// <inheritdoc/>
    public static string QueueName => GlobalStaticConstantsTransmission.TransmissionQueues.GetUsersOfIdentityReceive;

    static readonly TimeSpan _ts = TimeSpan.FromSeconds(2);

    /// <inheritdoc/>
    public async Task<TResponseModel<UserInfoModel[]>?> ResponseHandleActionAsync(string[]? users_ids = null, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(users_ids);
        users_ids = [.. users_ids.Where(x => !string.IsNullOrWhiteSpace(x) && x != GlobalStaticConstantsRoles.Roles.System).Distinct()];
        TResponseModel<UserInfoModel[]> res = new();
        if (users_ids.Length == 0)
        {
            res.AddError($"Пустой запрос > {nameof(ResponseHandleActionAsync)}");
            return new() { Response = res.Response, Messages = res.Messages };
        }

        return await identityRepo.GetUsersOfIdentityAsync(users_ids, token);
    }
}