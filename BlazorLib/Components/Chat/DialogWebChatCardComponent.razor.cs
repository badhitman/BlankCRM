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
    MessagesForWebChatComponent? messagesRef;

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
    DialogWebChatModelDB? currentDialog, dialogEdit;
    bool RoomIsEdit
    {
        get
        {
            if (currentDialog is null || dialogEdit is null)
                return false;

            bool isEq =
                ((string.IsNullOrWhiteSpace(currentDialog.InitiatorContacts) && string.IsNullOrWhiteSpace(dialogEdit.InitiatorContacts)) || currentDialog.InitiatorContacts == dialogEdit.InitiatorContacts) &&
                ((string.IsNullOrWhiteSpace(currentDialog.InitiatorHumanName) && string.IsNullOrWhiteSpace(dialogEdit.InitiatorHumanName)) || currentDialog.InitiatorHumanName == dialogEdit.InitiatorHumanName) &&
                ((string.IsNullOrWhiteSpace(currentDialog.InitiatorIdentityId) && string.IsNullOrWhiteSpace(dialogEdit.InitiatorIdentityId)) || currentDialog.InitiatorIdentityId == dialogEdit.InitiatorIdentityId);

            return !isEq;
        }
    }
    bool RoomNotEdit => !RoomIsEdit;


    async Task JoinToChat(bool isExclusive = false)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        TAuthRequestStandardModel<UserInjectDialogWebChatRequestModel> req = new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                UserIdentityId = CurrentUserSession.UserId,
                DialogJoinId = DialogId,
                IsExclusiveJoin = isExclusive,
            }
        };
        if (messagesRef is not null)
        {
            messagesRef.SoundIsMute = true;
            await SetBusyAsync();
            ResponseBaseModel res = await WebChatRepo.UserInjectDialogWebChatAsync(req);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
        }
        await ReloadDialog();
        await SetBusyAsync(false);
    }

    async Task SaveRoom()
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }
        if (dialogEdit is null)
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
                Id = dialogEdit.Id,
                InitiatorContacts = dialogEdit.InitiatorContacts,
                InitiatorHumanName = dialogEdit.InitiatorHumanName,
                InitiatorIdentityId = dialogEdit.InitiatorIdentityId,
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ReloadDialog();
        await SetBusyAsync(false);
    }

    async void SelectUserHandler(UserInfoModel? selected)
    {
        if (dialogEdit is null)
            return;

        dialogEdit.InitiatorIdentityId = selected?.UserId;
        StateHasChanged();
    }

    async Task ReloadDialog()
    {
        if (CurrentUserSession is null)
            throw new ArgumentNullException(nameof(CurrentUserSession));

        await SetBusyAsync();
        TResponseModel<List<DialogWebChatModelDB>> res = await WebChatRepo.DialogsWebChatsReadAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = [DialogId]
        });
        currentDialog = res.Response?.FirstOrDefault();
        dialogEdit = GlobalTools.CreateDeepCopy(currentDialog);

        if (currentDialog is null)
        {
            await SetBusyAsync(false);
            SnackBarRepo.Error("CurrentRoom is null");
            return;
        }

        if (!string.IsNullOrWhiteSpace(currentDialog?.InitiatorIdentityId))
            await CacheUsersUpdate([currentDialog.InitiatorIdentityId]);

        if (userSelectorRef is not null)
            await userSelectorRef.SetSelectedUser(dialogEdit?.InitiatorIdentityId);

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
        await ReloadDialog();
    }
    async void OnLoadStatusAction()
    {
        await InvokeAsync(StateHasChanged);
    }
}