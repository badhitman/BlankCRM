////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib;
using MudBlazor;
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
        phoneNumChangeRequest,
        kladrTitle,
        addressUserComment;

    List<ResultMessage> Messages = [];

    bool _visible;
    string? _editPhone;
    readonly DialogOptions _dialogOptions = new() { FullWidth = true };

    string GetTitleChangePhone()
    {
        if (_editPhone != phoneNum && string.IsNullOrWhiteSpace(_editPhone))
            return "Удалить номер";

        return "Отправить";
    }

    void OpenDialog()
    {
        _editPhone = phoneNum;
        _visible = true;
    }

    async Task DeletePhone()
    {
        _editPhone = null;
        await Submit();
    }

    async Task Submit()
    {
        if (!string.IsNullOrWhiteSpace(_editPhone) && !GlobalTools.IsPhoneNumber(_editPhone))
        {
            SnackBarRepo.Error("Телефон должен быть в формате: +79994440011 (можно без +)");
            return;
        }

        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        await SetBusyAsync();
        ResponseBaseModel res = await IdentityRepo.InitChangePhoneUserAsync(new TAuthRequestModel<string>() { SenderActionUserId = CurrentUserSession.UserId, Payload = _editPhone });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (!res.Success())
        {
            await SetBusyAsync(false);
            return;
        }

        _visible = false;
        _editPhone = null;
        await ReloadUserData(true);
    }


    bool IsEdited =>
        CurrentUserSession is not null &&
        (firstName != CurrentUserSession.GivenName || lastName != CurrentUserSession.Surname || patronymic != CurrentUserSession.Patronymic || phoneNum != CurrentUserSession.PhoneNumber);

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadUserData();
    }

    async Task ReloadUserData(bool readActualData = false)
    {
        if (readActualData)
            await ReadCurrentUser();

        await SetBusyAsync();
        if (CurrentUserSession is null)
            throw new Exception();

        firstName = CurrentUserSession.GivenName;
        lastName = CurrentUserSession.Surname;
        phoneNum = CurrentUserSession.PhoneNumber;
        username = CurrentUserSession.UserName;
        patronymic = CurrentUserSession.Patronymic;
        phoneNumChangeRequest = CurrentUserSession.RequestChangePhone;

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