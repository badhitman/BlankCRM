////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Retail.Clients;

/// <summary>
/// ClientAboutComponent
/// </summary>
public partial class ClientAboutComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IRetailService RetailRepo { get; set; } = default!;

    [Inject]
    ITelegramTransmission TelegramRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string ClientId { get; set; }


    UserInfoModel? currentUser, editClientCopy;
    ChatTelegramStandardModel? currentChatTelegram;
    List<WalletRetailModelDB>? WalletsForUser;

    EntryAltStandardModel? SelectedKladrObject
    {
        get => EntryAltStandardModel.Build(editClientCopy?.KladrCode ?? "", editClientCopy?.KladrTitle ?? "");
        set
        {
            if (editClientCopy is null)
                return;

            editClientCopy.KladrCode = value?.Id;
            editClientCopy.KladrTitle = value?.Name;
        }
    }

    bool CannotSaveUserDetails
    {
        get
        {
            if (currentUser is null || editClientCopy is null)
                return true;

            return
                currentUser.GivenName == editClientCopy.GivenName &&
                currentUser.Surname == editClientCopy.Surname &&
                currentUser.Patronymic == editClientCopy.Patronymic &&
                currentUser.ExternalUserId == editClientCopy.ExternalUserId &&
                currentUser.KladrTitle == editClientCopy.KladrTitle &&
                currentUser.KladrCode == editClientCopy.KladrCode &&
                currentUser.AddressUserComment == editClientCopy.AddressUserComment;
        }
    }

    bool CannotSaveUserPhone
        => currentUser is null ||
           editClientCopy is null ||
           currentUser.PhoneNumber == editClientCopy.PhoneNumber ||
           !GlobalTools.IsPhoneNumber(editClientCopy.PhoneNumber);


    void AddressUserCommentHandleOnChange(ChangeEventArgs args)
    {
        if (editClientCopy is null || editClientCopy is null)
        {
            SnackBarRepo.Error("editClientCopy is null || editClientCopy is null");
            return;
        }

        editClientCopy.AddressUserComment = args.Value?.ToString();
    }

    async Task SetPhone(string? phoneNum)
    {
        if (CurrentUserSession is null)
            return;

        if (!string.IsNullOrWhiteSpace(phoneNum) && !GlobalTools.IsPhoneNumber(phoneNum))
        {
            SnackBarRepo.Error("Телефон должен быть в формате: +79994440011 (можно без +)");
            return;
        }

        TAuthRequestStandardModel<ChangePhoneUserRequestModel> req = new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                UserId = ClientId,
                PhoneNum = phoneNum
            }
        };

        await SetBusyAsync();
        ResponseBaseModel res = await IdentityRepo.ConfirmChangePhoneUserAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (res.Success())
        {
            TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([ClientId]);
            SnackBarRepo.ShowMessagesResponse(getUser.Messages);
            currentUser = getUser.Response?.FirstOrDefault(x => x.UserId == ClientId);
            editClientCopy = GlobalTools.CreateDeepCopy(currentUser);
        }

        await SetBusyAsync(false);
    }

    async Task SaveUserDetails()
    {
        if (editClientCopy is null)
            return;

        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        IdentityDetailsModel req = new()
        {
            UserId = ClientId,
            AddressUserComment = editClientCopy.AddressUserComment,
            KladrCode = editClientCopy.KladrCode,
            KladrTitle = editClientCopy.KladrTitle,
            Patronymic = editClientCopy.Patronymic,
            FirstName = editClientCopy.GivenName,
            LastName = editClientCopy.Surname,
            ExternalUserId = editClientCopy.ExternalUserId,
            UpdateAddress = true,
        };

        await SetBusyAsync();
        ResponseBaseModel res = await IdentityRepo.UpdateUserDetailsAsync(new() { SenderActionUserId = CurrentUserSession.UserId, Payload = req });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (res.Success())
        {
            TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([ClientId]);
            SnackBarRepo.ShowMessagesResponse(getUser.Messages);
            currentUser = getUser.Response?.FirstOrDefault(x => x.UserId == ClientId);
            editClientCopy = GlobalTools.CreateDeepCopy(currentUser);
        }

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected async override Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        TResponseModel<UserInfoModel[]> getUser = await IdentityRepo.GetUsersOfIdentityAsync([ClientId]);
        SnackBarRepo.ShowMessagesResponse(getUser.Messages);
        currentUser = getUser.Response?.FirstOrDefault(x => x.UserId == ClientId);
        if (currentUser is not null)
        {
            editClientCopy = GlobalTools.CreateDeepCopy(currentUser);
            if (currentUser.TelegramId.HasValue)
            {
                List<ChatTelegramStandardModel> chats = await TelegramRepo.ChatsReadTelegramAsync([currentUser.TelegramId.Value]);
                currentChatTelegram = chats.FirstOrDefault();
            }
        }

        TPaginationRequestStandardModel<SelectWalletsRetailsRequestModel> reqWallets = new()
        {
            PageNum = 0,
            PageSize = int.MaxValue,
            Payload = new()
            {
                UsersFilterIdentityId = [ClientId],
                AutoGenerationWallets = true,
            }
        };
        TPaginationResponseStandardModel<WalletRetailModelDB>? resWallets = await RetailRepo.SelectWalletsAsync(reqWallets);
        WalletsForUser = resWallets.Response;

        await SetBusyAsync(false);
    }
}