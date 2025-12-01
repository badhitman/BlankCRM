////////////////////////////////////////////////
// © https://github.com/badhitman 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using SharedLib;
using Newtonsoft.Json;

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
    public async Task ReadCurrentUser()
    {
        AuthenticationState state = await AuthRepo.GetAuthenticationStateAsync();
        UserInfoMainModel? _usr = state.User.ReadCurrentUserInfo();

        if (_usr is null)
        {
            CurrentUserSession = null;
            return;
        }

        TResponseModel<UserInfoModel[]> getDataUser = await IdentityRepo.GetUsersOfIdentityAsync([_usr.UserId]);
        SnackBarRepo.ShowMessagesResponse(getDataUser.Messages);
        if (getDataUser.Response is null || getDataUser.Response.Length != 1)
            throw new Exception(JsonConvert.SerializeObject(getDataUser));

        CurrentUserSession = getDataUser.Response[0];
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
    }
}