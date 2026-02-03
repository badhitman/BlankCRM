////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Chat;

public partial class DialogWebChatCardComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public int DialogId { get; set; }

    DialogWebChatModelDB? CurrentRoom;


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