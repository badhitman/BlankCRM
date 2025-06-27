﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using System.Net.Mail;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Account.Pages.Manage;

/// <summary>
/// EmailPage
/// </summary>
public partial class EmailPage : BlazorBusyComponentBaseAuthModel
{
    [SupplyParameterFromForm(FormName = "change-email")]
    private NewEmailSingleModel Input { get; set; } = new();

    private string? email;
    private bool isEmailConfirmed;
    IEnumerable<ResultMessage>? Messages;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await ReadCurrentUser();
        if (CurrentUserSession is null)
            throw new Exception($"Пользователь сессии не определён");

        TResponseModel<bool> rest = await UsersProfilesRepo.IsEmailConfirmedAsync();
        Messages = rest.Messages;

        email = CurrentUserSession.Email;
        isEmailConfirmed = rest.Response == true;
        Input.NewEmail ??= email;
    }

    private async Task OnValidSubmitAsync()
    {
        if (Input.NewEmail is null || Input.NewEmail == email)
        {
            Messages = [new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = "Ваш адрес электронной почты не изменился." }];
            return;
        }

        ResponseBaseModel rest = await UsersProfilesRepo.GenerateChangeEmailTokenAsync(Input.NewEmail, NavigationManager.ToAbsoluteUri("Account/ConfirmEmailChange").AbsoluteUri);
        Messages = rest.Messages;
    }

    private async Task OnSendEmailVerificationAsync()
    {
        if (!MailAddress.TryCreate(email, out _))
        {
            Messages = [new ResultMessage() { TypeMessage = MessagesTypesEnum.Error, Text = $"email [{email}] имеет не корректный формат" }];
            return;
        }
        ResponseBaseModel rest = await IdentityRepo.GenerateEmailConfirmationAsync(new() { Email = email, BaseAddress = NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri });
        Messages = rest.Messages;
    }
}