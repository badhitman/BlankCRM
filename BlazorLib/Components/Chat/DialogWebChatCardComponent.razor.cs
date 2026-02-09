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

    [Inject]
    IEventsWebChatsNotifies EventsWebChatsHandleRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public int DialogId { get; set; }


    ChatStatusComponent? chatStatusRef;

    async Task SetStateChatRequest(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
            => await EventsWebChatsHandleRepo.StateSetWebChatAsync(new StateWebChatModel() { DialogId = DialogId, StateDialog = true });
    async Task StateChatRequest(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        chatStatusRef = null;
        await SetBusyAsync();
        await EventsWebChatsHandleRepo.StateGetWebChatAsync(new GetStateWebChatEventModel() { DialogId = DialogId });
        await SetBusyAsync(false);
    }

    UserSelectInputComponent? userSelectorRef;
    DialogWebChatModelDB? CurrentDialog, roomEdit;
    bool RoomIsEdit
    {
        get
        {
            if (CurrentDialog is null || roomEdit is null)
                return false;

            bool isEq =
                ((string.IsNullOrWhiteSpace(CurrentDialog.InitiatorContacts) && string.IsNullOrWhiteSpace(roomEdit.InitiatorContacts)) || CurrentDialog.InitiatorContacts == roomEdit.InitiatorContacts) &&
                ((string.IsNullOrWhiteSpace(CurrentDialog.InitiatorHumanName) && string.IsNullOrWhiteSpace(roomEdit.InitiatorHumanName)) || CurrentDialog.InitiatorHumanName == roomEdit.InitiatorHumanName) &&
                ((string.IsNullOrWhiteSpace(CurrentDialog.InitiatorIdentityId) && string.IsNullOrWhiteSpace(roomEdit.InitiatorIdentityId)) || CurrentDialog.InitiatorIdentityId == roomEdit.InitiatorIdentityId);

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
        CurrentDialog = res.Response?.FirstOrDefault();
        roomEdit = GlobalTools.CreateDeepCopy(CurrentDialog);

        if(CurrentDialog is null)
        {
            await SetBusyAsync(false);
            SnackBarRepo.Error("CurrentRoom is null");
            return;
        }

        if (!string.IsNullOrWhiteSpace(CurrentDialog?.InitiatorIdentityId))
            await CacheUsersUpdate([CurrentDialog.InitiatorIdentityId]);

        if (userSelectorRef is not null)
            await userSelectorRef.SetSelectedUser(roomEdit?.InitiatorIdentityId);

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
    async void OnLoadStatusAction()
    {
        await InvokeAsync(StateHasChanged);
    }
}