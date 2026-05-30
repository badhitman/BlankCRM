////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using BlazorLib;

namespace BlazorWebLib.Components.Account.Pages.Manage;

/// <summary>
/// ChangePasswordPage
/// </summary>
public partial class ChangePasswordPage
{
    [Inject]
    IdentityRedirectManager RedirectManager { get; set; } = default!;

    [Inject]
    IUsersProfilesService UsersProfilesRepo { get; set; } = default!;

    [SupplyParameterFromForm]
    ChangePasswordModel? Input { get; set; }


    IEnumerable<ResultMessage>? Messages;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        Input ??= new();
        TResponseModel<bool?> rest = await UsersProfilesRepo.UserHasPasswordAsync();
        //user = rest.UserInfo;
        Messages = rest.Messages;
        if (rest.Response != true)
        {
            RedirectManager.RedirectTo("Account/Manage/SetPassword");
        }
    }

    private async Task OnValidSubmitAsync()
    {
        if (Input is null)
            throw new ArgumentNullException(nameof(Input));

        Messages = null;
        ResponseBaseModel changePasswordResult = await UsersProfilesRepo.ChangePasswordAsync(Input.OldPassword, Input.NewPassword);
        Messages = changePasswordResult.Messages;
        if (!changePasswordResult.Success())
            return;

        Messages = [new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = "Ваш пароль был изменен" }];
    }
}