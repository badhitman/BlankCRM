////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Account.Pages.Manage;

/// <summary>
/// SetPasswordPage
/// </summary>
public partial class SetPasswordPage : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IdentityRedirectManager RedirectManager { get; set; } = default!;

    [Inject]
    IUsersProfilesService UsersProfilesRepo { get; set; } = default!;


    [SupplyParameterFromForm]
    SetNewPasswordModel? Input { get; set; }

    List<ResultMessage>? Messages;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        Input ??= new();
        await base.OnInitializedAsync();

        TResponseModel<bool?> hasPassword = await UsersProfilesRepo.UserHasPasswordAsync();
        Messages = hasPassword.Messages;
        if (hasPassword.Response == true || CurrentUserSession is null)
        {
            RedirectManager.RedirectTo("Account/Manage/ChangePassword");
        }
    }

    private async Task OnValidSubmitAsync()
    {
        if (Input is null)
            throw new ArgumentNullException(nameof(Input));

        Messages = null;
        ResponseBaseModel rest = await UsersProfilesRepo.AddPasswordAsync(Input.NewPassword!);
        Messages = rest.Messages;
        if (!rest.Success())
            return;
        Messages.Add(new() { TypeMessage = MessagesTypesEnum.Warning, Text = "Ваш пароль установлен." });
    }
}