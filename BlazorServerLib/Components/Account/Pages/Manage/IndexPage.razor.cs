////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Account.Pages.Manage;

/// <summary>
/// IndexPage
/// </summary>
public partial class IndexPage : BlazorBusyComponentBaseAuthModel
{



    string?
        username,
        firstName,
        lastName,
        patronymic,
        phoneNum,
        kladrTitle,
        addressUserComment;

    List<ResultMessage> Messages = [];

    bool IsEdited =>
        CurrentUserSession is not null &&
        (firstName != CurrentUserSession.GivenName || lastName != CurrentUserSession.Surname || patronymic != CurrentUserSession.Patronymic || phoneNum != CurrentUserSession.PhoneNumber);

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await ReadCurrentUser();

        if (CurrentUserSession is null)
            throw new Exception();

        firstName = CurrentUserSession.GivenName;
        lastName = CurrentUserSession.Surname;
        phoneNum = CurrentUserSession.PhoneNumber;
        username = CurrentUserSession.UserName;
        patronymic = CurrentUserSession.Patronymic;


        kladrTitle = CurrentUserSession.KladrTitle;
        addressUserComment = CurrentUserSession.AddressUserComment;
        await SetBusyAsync(false);
    }

    private async Task SaveAsync()
    {
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        if (!string.IsNullOrWhiteSpace(phoneNum) && !GlobalTools.IsPhoneNumber(phoneNum))
        {
            SnackBarRepo.Error("Телефон должен быть в формате: +79994440011 (можно без +)");
            return;
        }

        Messages = [];
        await SetBusyAsync();

        ResponseBaseModel rest = await IdentityRepo.UpdateUserDetailsAsync(new IdentityDetailsModel()
        {
            UserId = CurrentUserSession.UserId,
            FirstName = firstName,
            LastName = lastName,
            PhoneNum = phoneNum,
            Patronymic = patronymic,
        });

        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(rest.Messages);
        if (rest.Success())
        {
            CurrentUserSession.GivenName = firstName;
            CurrentUserSession.Surname = lastName;
            CurrentUserSession.PhoneNumber = phoneNum;
        }
    }
}