////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Text;
using SharedLib;

namespace BlazorLib;

/// <summary>
/// BlazorBusyComponentBaseAuthModel
/// </summary>
public abstract class BlazorBusyComponentBaseAuthModel : BlazorBusyComponentBaseModel
{
    [Inject]
    AuthenticationStateProvider AuthRepo { get; set; } = default!;

    /// <inheritdoc/>
    [Inject]
    protected IIdentityTransmission IdentityRepo { get; set; } = default!;


    /// <summary>
    /// Текущий пользователь (сессия)
    /// </summary>
    public UserInfoModel? CurrentUserSession { get; private set; }


    /// <inheritdoc/>
    public byte[]? CurrentUserSessionBytes(string layoutContainerId) => CurrentUserSession is null
        ? null
        : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new KeyValuePair<string, UserInfoModel?>(layoutContainerId, CurrentUserSession), GlobalStaticConstants.JsonSerializerSettings));

    /// <inheritdoc/>
    public async Task ReadCurrentUser()
    {
        AuthenticationState state = await AuthRepo.GetAuthenticationStateAsync();
        UserInfoMainModel? _usr = state.User.ReadCurrentUserInfo();

        if (_usr is null)
        {
            CurrentUserSession = null;
            return;
        }
        await SetBusyAsync();
        TResponseModel<UserInfoModel[]> getDataUser = await IdentityRepo.GetUsersOfIdentityAsync([_usr.UserId]);
        SnackBarRepo.ShowMessagesResponse(getDataUser.Messages);
        if (getDataUser.Response is null || !getDataUser.Response.Any(x => x.UserId == _usr.UserId))
            throw new Exception(JsonConvert.SerializeObject(getDataUser, Formatting.Indented, GlobalStaticConstants.JsonSerializerSettings));

        CurrentUserSession = getDataUser.Response.First(x => x.UserId == _usr.UserId);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
    }
}