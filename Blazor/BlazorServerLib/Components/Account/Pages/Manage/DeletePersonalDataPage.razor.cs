////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorWebLib.Components.Account.Pages.Manage;

/// <summary>
/// DeletePersonalDataPage
/// </summary>
public partial class DeletePersonalDataPage : BlazorBusyComponentBaseAuthModel
{
    [SupplyParameterFromForm]
    PasswordSingleModel? Input { get; set; }


    bool requirePassword;

    List<ResultMessage> Messages = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        Input ??= new();
        await base.OnInitializedAsync();
        var rest = await UsersProfilesRepo.UserHasPasswordAsync();
        Messages = rest.Messages;
        requirePassword = rest.Response == true;
    }

    private async Task OnValidSubmitAsync()
    {
        if (Input is null)
            throw new ArgumentNullException(nameof(Input));

        var rest = await UsersProfilesRepo.CheckUserPasswordAsync(Input.Password);
        Messages = rest.Messages;
        if (requirePassword && !rest.Success())
            return;

        ResponseBaseModel result = await UsersProfilesRepo.DeleteUserDataAsync(Input.Password);
        Messages.AddRange(result.Messages);
        if (!result.Success())
        {
            throw new InvalidOperationException("Произошла непредвиденная ошибка при удалении пользователя.");
        }

        await UserAuthRepo.SignOutAsync();
        RedirectManager.RedirectToCurrentPage();
    }
}