////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// DialogWebChatCardComponent
/// </summary>
public partial class DialogWebChatCardComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public int DialogId { get; set; }


    DialogWebChatModelDB? CurrentRoom, roomEdit;
    bool RoomIsEdit
    {
        get
        {
            if (CurrentRoom is null || roomEdit is null)
                return false;

            bool isEq =
                ((string.IsNullOrWhiteSpace(CurrentRoom.InitiatorContacts) && string.IsNullOrWhiteSpace(roomEdit.InitiatorContacts)) || CurrentRoom.InitiatorContacts == roomEdit.InitiatorContacts) &&
                ((string.IsNullOrWhiteSpace(CurrentRoom.InitiatorHumanName) && string.IsNullOrWhiteSpace(roomEdit.InitiatorHumanName)) || CurrentRoom.InitiatorHumanName == roomEdit.InitiatorHumanName) &&
                ((string.IsNullOrWhiteSpace(CurrentRoom.InitiatorIdentityId) && string.IsNullOrWhiteSpace(roomEdit.InitiatorIdentityId)) || CurrentRoom.InitiatorIdentityId == roomEdit.InitiatorIdentityId);

            return !isEq;
        }
    }
    bool RoomNotEdit => !RoomIsEdit;

    async Task SaveRoom()
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
        if (roomEdit is null)
        {
            SnackBarRepo.Error("roomEdit is null");
            return;
        }

        await SetBusyAsync();
        ResponseBaseModel res = await WebChatRepo.UpdateDialogWebChatAdminAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                Id = roomEdit.Id,
                InitiatorContacts = roomEdit.InitiatorContacts,
                InitiatorHumanName = roomEdit.InitiatorHumanName,
                InitiatorIdentityId = roomEdit.InitiatorIdentityId,
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ReloadRoom();
        await SetBusyAsync(false);
    }

    async void SelectUserHandler(UserInfoModel? selected)
    {
        if (roomEdit is null)
            return;

        roomEdit.InitiatorIdentityId = selected?.UserId;
        StateHasChanged();
    }

    async Task ReloadRoom()
    {
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        await SetBusyAsync();
        TResponseModel<List<DialogWebChatModelDB>> res = await WebChatRepo.DialogsWebChatsReadAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = [DialogId]
        });
        CurrentRoom = res.Response?.First();
        roomEdit = GlobalTools.CreateDeepCopy(CurrentRoom);

        if (!string.IsNullOrWhiteSpace(CurrentRoom?.InitiatorIdentityId))
            await CacheUsersUpdate([CurrentRoom.InitiatorIdentityId]);

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        UsersCache.Add(CurrentUserSession);
        await ReloadRoom();
    }
}