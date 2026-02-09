////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// WebChatsManageComponent
/// </summary>
public partial class WebChatsManageComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;

    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<NewMessageWebChatEventModel> NewMessageWebChatEventRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string? FilterUserIdentityId { get; set; }


    MudTable<DialogWebChatModelDB>? tableRef;
    bool muteSound;
    readonly string LayoutContainerId = Guid.NewGuid().ToString();

    bool? _anotherValue;
    bool? AnotherValue
    {
        get => _anotherValue;
        set
        {
            _anotherValue = value;
            if (tableRef is not null)
                InvokeAsync(tableRef.ReloadServerData);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await NewMessageWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatNotifyReceive, "#"), NewMessageWebChatHandler, LayoutContainerId, CurrentUserSessionBytes);
    }

    void NewMessageWebChatHandler(NewMessageWebChatEventModel model)
    {
        if (tableRef is not null)
            InvokeAsync(async () =>
            {
                await tableRef.ReloadServerData();
                StateHasChanged();
                if (!muteSound)
                    await JsRuntime.InvokeVoidAsync("methods.PlayAudio", "WebChatsManageComponent");
                else
                    muteSound = false;
            });
    }

    async Task JoinToChat(int chatId, bool isExclusive = false)
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
                DialogJoinId = chatId,
                IsExclusiveJoin = isExclusive,
            }
        };
        muteSound = true;
        await SetBusyAsync();
        ResponseBaseModel res = await WebChatRepo.UserInjectDialogWebChatAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async Task OutFromChat(int chatId)
    {
        if (CurrentUserSession is null)
        {
            SnackBarRepo.Error("CurrentUserSession is null");
            return;
        }

        TAuthRequestStandardModel<int> req = new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = chatId,
        };
        muteSound = true;
        await SetBusyAsync();
        ResponseBaseModel res = await WebChatRepo.DeleteUserJoinDialogWebChatAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (tableRef is not null)
            await tableRef.ReloadServerData();

        await SetBusyAsync(false);
    }

    async Task<TableData<DialogWebChatModelDB>> ServerReload(TableState state, CancellationToken token)
    {
        TPaginationRequestStandardModel<SelectDialogsWebChatsRequestModel> req = new()
        {
            PageNum = state.Page,
            PageSize = state.PageSize,
            SortingDirection = state.SortDirection.Convert(),
            Payload = new()
            {
                FilterUserIdentityId = FilterUserIdentityId,
                IsDisabledFiltered = AnotherValue,
            }
        };
        await SetBusyAsync(token: token);
        TPaginationResponseStandardModel<DialogWebChatModelDB> res = await WebChatRepo.SelectDialogsWebChatsAsync(req, token);

        if (res.Response is not null && res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Where(x => !string.IsNullOrWhiteSpace(x.InitiatorIdentityId)).Select(x => x.InitiatorIdentityId)!]);

        await SetBusyAsync(false, token);
        SnackBarRepo.ShowMessagesResponse(res.Status.Messages);
        return new TableData<DialogWebChatModelDB>() { TotalItems = res.TotalRowsCount, Items = res.Response };
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        NewMessageWebChatEventRepo.UnregisterAction();
    }
}