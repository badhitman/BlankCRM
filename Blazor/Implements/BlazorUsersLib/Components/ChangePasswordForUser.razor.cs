////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using BlazorLib;
using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorUsersLib.Components;

/// <summary>
/// ChangePasswordForUser
/// </summary>
public partial class ChangePasswordForUser : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IUsersProfilesService UsersProfilesRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string OwnerId { get; set; }

    PasswordBaseModel Input { get; set; } = new();

    async Task OnValidSubmitAsync()
    {
        if (Input is null)
            throw new ArgumentNullException(nameof(Input));

        ResponseBaseModel changePasswordResult = await UsersProfilesRepo.ChangePasswordAsync(Input.Password, Input.ConfirmPassword);
        SnackBarRepo.ShowMessagesResponse(changePasswordResult.Messages);

        if (changePasswordResult.Success())
        {
            Input.Password = "";
            Input.ConfirmPassword = "";
        }
    }
}