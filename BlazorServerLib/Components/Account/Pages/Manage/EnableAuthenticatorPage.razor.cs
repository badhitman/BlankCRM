﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Account.Pages.Manage;

/// <summary>
/// EnableAuthenticatorPage
/// </summary>
public partial class EnableAuthenticatorPage : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    AuthenticationStateProvider AuthRepo { get; set; } = default!;

    [SupplyParameterFromForm]
    private CodeSingleModel Input { get; set; } = new();

    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    private string? message;
    private string? sharedKey;
    private string? authenticatorUri;
    private IEnumerable<string>? recoveryCodes;

    List<ResultMessage> Messages = [];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await LoadSharedKeyAndQrCodeUriAsync();
    }

    private async Task OnValidSubmitAsync()
    {
        // Strip spaces and hyphens
        string verificationCode = Input.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
        var is_2fa_token_valid_rest = await UsersProfilesRepo.VerifyTwoFactorTokenAsync(verificationCode);
        Messages = is_2fa_token_valid_rest.Messages;
        if (!is_2fa_token_valid_rest.Success())
        {
            Messages.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = "Ошибка: код подтверждения недействителен." });
            return;
        }

        ResponseBaseModel stf_res = await UsersProfilesRepo.SetTwoFactorEnabledAsync(true);
        Messages.AddRange(stf_res.Messages);
        if (!stf_res.Success())
            return;

        Messages.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Success, Text = "Ваше приложение-аутентификатор проверено." });

        TResponseModel<int?> cnt_rc = await UsersProfilesRepo.CountRecoveryCodesAsync();
        Messages.AddRange(cnt_rc.Messages);
        if (!cnt_rc.Success() || cnt_rc.Response is null)
            return;

        if (cnt_rc.Response.Value == 0)
        {
            TResponseModel<IEnumerable<string>?> rc_res = await UsersProfilesRepo.GenerateNewTwoFactorRecoveryCodesAsync();
            Messages.AddRange(rc_res.Messages);
            recoveryCodes = rc_res.Response;
        }
        else
        {
            message = "Ваше приложение-аутентификатор проверено.";
            Messages.Add(new() { TypeMessage = MessagesTypesEnum.Info, Text = message });
        }
    }

    private async ValueTask LoadSharedKeyAndQrCodeUriAsync()
    {
        string msg;
        if (!MailAddress.TryCreate(CurrentUserSession!.Email, out _))
        {
            msg = "email имеет не корректный формат. error {F0A2EF98-5C04-46D2-8A93-711BC9827EF9}";
            Messages.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = msg });
            throw new Exception(msg);
        }

        // Load the authenticator key & QR code URI to display on the form
        TResponseModel<string?> unformatted_key_rest = await UsersProfilesRepo.GetAuthenticatorKeyAsync();
        if (string.IsNullOrEmpty(unformatted_key_rest.Response))
        {
            msg = "string.IsNullOrEmpty(unformatted_key_rest.ResponseString). error {F0A2EF98-5C04-46D2-8A93-711BC9827EF9}";
            Messages.Add(new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = msg });
            throw new Exception(msg);
        }

        sharedKey = FormatKey(unformatted_key_rest.Response);
        authenticatorUri = GenerateQrCodeUri(CurrentUserSession.Email, unformatted_key_rest.Response);
    }

    private static string FormatKey(string unformattedKey)
    {
        StringBuilder result = new();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.AsSpan(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            AuthenticatorUriFormat,
            UrlEncoder.Encode("Microsoft.AspNetCore.Identity.UI"),
            UrlEncoder.Encode(email),
            unformattedKey);
    }
}