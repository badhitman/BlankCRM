﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using SharedLib;
using BlazorLib;

namespace BlazorWebLib.Components.Account.Pages;

/// <summary>
/// LoginPage
/// </summary>
public partial class LoginPage(IUsersAuthenticateService UserAuthManage, NavigationManager NavigationManager, IdentityRedirectManager RedirectManager)
{
    [Inject]
    IUsersAuthenticateService AuthRepo { get; set; } = default!;

    [Inject]
    ILogger<LoginPage> Logger { get; set; } = default!;


    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery]
    private string? TwoFactorCode { get; set; }

    [SupplyParameterFromQuery]
    private string? UserAlias { get; set; }

    [SupplyParameterFromForm]
    // #if DEMO
    //     private UserAuthorizationModel Input { get; set; } = new() { Password = "Qwerty123!" };
    //     bool IsDebug = true;
    // #else
    private UserAuthorizationModel Input { get; set; } = new();
    bool IsDebug = false;
    //#endif


    /// <summary>
    /// Это не учитывает ошибки входа в систему при блокировке учетной записи.
    /// Чтобы включить блокировку учетной записи при сбое пароля, установите lockoutOnFailure: true
    /// </summary>
    SignInResultResponseModel? result;

    List<ResultMessage> Messages = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        if (HttpMethods.IsGet(HttpContext.Request.Method) && HttpContext.User.Identity?.IsAuthenticated == true)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }
        if (!string.IsNullOrWhiteSpace(TwoFactorCode) && !string.IsNullOrWhiteSpace(UserAlias))
            await Login2FA();
    }

    /// <summary>
    /// LoginUser
    /// </summary>
    public async Task OnValidSubmit()
    {
        result = await UserAuthManage.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe);
        Messages.AddRange(result.Messages);
        if (result.RequiresTwoFactor == true)
            return;

        if (result.Success())
            RedirectManager.RedirectTo(ReturnUrl);
        else if (result.IsLockedOut == true)
            RedirectManager.RedirectTo("Account/Lockout");
    }

    private async Task Login2FA()
    {
        if (string.IsNullOrWhiteSpace(TwoFactorCode) || string.IsNullOrWhiteSpace(UserAlias))
            return;

        string authenticatorCode = TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
        IdentityResultResponseModel result = await AuthRepo.TwoFactorAuthenticatorSignInAsync(authenticatorCode, true, Input.RememberMe, UserAlias);

        if (result.Succeeded == true)
        {
            Logger.LogInformation("User with logged in with 2fa.");
            RedirectManager.RedirectTo(ReturnUrl);
        }
        else if (result.IsLockedOut == true)
        {
            Logger.LogWarning("User with ID account locked out.");
            RedirectManager.RedirectTo("Account/Lockout");
        }
        else
        {
            Logger.LogWarning("Invalid authenticator code entered.");
            Messages =[new() { TypeMessage = MessagesTypesEnum.Error, Text = "Неверный код аутентификации." }];
        }
    }
}