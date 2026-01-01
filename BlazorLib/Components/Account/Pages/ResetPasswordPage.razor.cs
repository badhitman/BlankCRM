////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using HtmlGenerator.html5.forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using SharedLib;
using System.Text;

namespace BlazorLib.Components.Account.Pages;

/// <summary>
/// ResetPasswordPage
/// </summary>
public partial class ResetPasswordPage
{
    [Inject]
    IdentityRedirectManager RedirectManager { get; set; } = default!;

    [Inject]
    IIdentityTransmission IdentityRepo { get; set; } = default!;


    [SupplyParameterFromForm]
    private LoginWithCodeModel? Input { get; set; }

    [SupplyParameterFromQuery]
    private string? Code { get; set; }


    IEnumerable<ResultMessage>? Messages;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if (Code is null)
        {
            RedirectManager.RedirectTo("Account/InvalidPasswordReset");
        }
        Input ??= new();
        Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
    }

    private async Task OnValidSubmitAsync()
    {
        if (Input is null)
            throw new ArgumentNullException(nameof(Input));

        TResponseModel<UserInfoModel> user = await IdentityRepo.FindUserByEmailAsync(Input.Email);
        if (user.Response is null)
        {
            // Don't reveal that the user does not exist
            RedirectManager.RedirectTo("Account/ResetPasswordConfirmation");
        }

        ResponseBaseModel result = await IdentityRepo.ResetPasswordAsync(new() { Password = Input.Password, Token = Input.Code, UserId = user.Response.UserId });
        Messages = result.Messages;
        if (result.Success())
        {
            RedirectManager.RedirectTo("Account/ResetPasswordConfirmation");
        }
    }
}