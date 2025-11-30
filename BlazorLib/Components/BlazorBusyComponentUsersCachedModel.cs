////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using MudBlazor;
using SharedLib;

namespace BlazorLib;

/// <summary>
/// BlazorBusyComponentUsersCachedModel
/// </summary>
public abstract class BlazorBusyComponentUsersCachedModel : BlazorBusyComponentBaseAuthModel
{
    /// <summary>
    /// UsersCache
    /// </summary>
    protected List<UserInfoModel> UsersCache = [];


    /// <summary>
    /// CacheUsersUpdate
    /// </summary>
    protected async Task CacheUsersUpdate(string[] usersIds)
    {
        usersIds = [..usersIds.Where(x => !string.IsNullOrWhiteSpace(x) && !UsersCache.Any(y => y.UserId == x)).Distinct()];
        if (usersIds.Length == 0)
            return;

        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> users = await IdentityRepo.GetUsersOfIdentityAsync(usersIds);
        SnackBarRepo.ShowMessagesResponse(users.Messages);
        if (users.Success() && users.Response is not null && users.Response.Length != 0)
            lock (UsersCache)
            {
                UsersCache.AddRange(users.Response.Where(x => !UsersCache.Any(y => y.UserId == x.UserId)));
            }

        await SetBusyAsync(false);
    }
}