////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Web.Virtualization;
using static SharedLib.GlobalStaticConstantsRoutes;
using BlazorLib.Components.Shared.Layouts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// ChatWrapperComponent
/// </summary>
public partial class ChatWrapperComponent : BlazorBusyComponentUsersCachedModel
{
    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;

    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;

    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IEventsWebChatsNotifies EventsWebChatsHandleRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<NewMessageWebChatEventModel> NewMessageWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<GetStateWebChatEventModel> StateGetWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<StateWebChatModel> StateSetWebChatEventRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required RealtimeCoreComponent RealtimeCore { get; set; }


    bool muteSound;
    MessageWebChatModelDB? _selectedMessage;
    AboutUserAgentModel? UserAgent;
    MudMenu? _contextMenu;
    DialogWebChatModelDB? dialogSession, dialogSessionEdit;
    string? lastUserId;
    Virtualize<MessageWebChatModelDB>? virtualizeComponent;
    string? _textSendMessage;
    byte missingMessages;
    bool CannotSendMessage => string.IsNullOrWhiteSpace(_textSendMessage) || IsBusyProgress;
    static readonly int virtualCacheSize = 50;
    readonly List<PongClientsWebChatEventModel> UsersSessions = [];
    readonly string LayoutContainerId = Guid.NewGuid().ToString();

    /// <inheritdoc/>
    public bool ChatDialogOpen { get; private set; }

    async Task ShowToggle()
    {
        ChatDialogOpen = !ChatDialogOpen;

        if (ChatDialogOpen)
        {
            missingMessages = 0;
            await InitSession();
            await RealtimeCore.PingAllUsers();
        }

        if (dialogSession is not null)
            await EventsWebChatsHandleRepo.StateEchoWebChatAsync(new StateWebChatModel()
            {
                StateDialog = ChatDialogOpen,
                DialogId = dialogSession.Id,
                UserIdentityId = CurrentUserSession?.UserId,
            });
    }

    async ValueTask<ItemsProviderResult<MessageWebChatModelDB>> LoadMessages(ItemsProviderRequest request)
    {
        if (dialogSession is null)
            return new ItemsProviderResult<MessageWebChatModelDB>([], 0);

        SelectMessagesForWebChatRequestModel req = new()
        {
            SessionTicketId = dialogSession.SessionTicketId,
            StartIndex = request.StartIndex,
            Count = request.Count,
        };
        await SetBusyAsync(token: request.CancellationToken);
        TResponseModel<SelectMessagesForWebChatResponseModel> res = await WebChatRepo.SelectMessagesWebChatAsync(req, request.CancellationToken);
        await SetBusyAsync(false, token: request.CancellationToken);
        if (res.Response is null)
            return new ItemsProviderResult<MessageWebChatModelDB>([], 0);

        if (res.Response.Count != 0)
            await CacheUsersUpdate([.. res.Response.Messages.Where(x => !string.IsNullOrWhiteSpace(x.SenderUserIdentityId)).Select(x => x.SenderUserIdentityId)!]);

        return new ItemsProviderResult<MessageWebChatModelDB>(res.Response.Messages, res.Response.TotalRowsCount);
    }

    async Task SevaDialog(MouseEventArgs args)
    {
        if (dialogSessionEdit is null)
            return;

        await SetBusyAsync();
        ResponseBaseModel res = await WebChatRepo.UpdateDialogWebChatInitiatorAsync(new()
        {
            SenderActionUserId = CurrentUserSession?.UserId,
            Payload = dialogSessionEdit
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await InitSession();

        await SetBusyAsync(false);
    }

    async Task SendMessage(MouseEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage) || dialogSession is null)
            return;

        MessageWebChatModelDB req = new()
        {
            Text = _textSendMessage.Trim(),
            SenderUserIdentityId = CurrentUserSession?.UserId,
            DialogOwnerId = dialogSession.Id,
            InitiatorMessageSender = true,
        };

        muteSound = true;
        await SetBusyAsync();
        await WebChatRepo.CreateMessageWebChatAsync(req);
        _textSendMessage = null;

        await SetBusyAsync(false);
    }

    async Task OnKeyPresHandler(KeyboardEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage) || dialogSession is null)
            return;

        if (args.Key == "Enter" && !args.ShiftKey)
        {
            MessageWebChatModelDB req = new()
            {
                Text = _textSendMessage.Trim(),
                SenderUserIdentityId = CurrentUserSession?.UserId,
                DialogOwnerId = dialogSession.Id,
                InitiatorMessageSender = true,
            };
            muteSound = true;
            await SetBusyAsync();
            await WebChatRepo.CreateMessageWebChatAsync(req);
            _textSendMessage = null;
            await SetBusyAsync(false);
        }
    }

    async Task InitSession()
    {
        UserAgent = await JsRuntime.InvokeAsync<AboutUserAgentModel?>("methods.AboutUserAgent");
        if (UserAgent?.CookieEnabled != true)
            return;

        string
            _sessionCookieName = Path.Combine(Routes.TICKET_CONTROLLER_NAME, Routes.SESSION_CONTROLLER_NAME).Replace("\\", "/"),
            _lastUserIdCookieName = Path.Combine(_sessionCookieName, $"{Routes.USER_CONTROLLER_NAME}-{Routes.IDENTITY_CONTROLLER_NAME}").Replace("\\", "/");

        lastUserId = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _lastUserIdCookieName);
        if (!string.IsNullOrWhiteSpace(CurrentUserSession?.UserId))
        {
            if (lastUserId != _lastUserIdCookieName)
                await JsRuntime.InvokeVoidAsync("methods.CreateCookie", _lastUserIdCookieName, CurrentUserSession.UserId, 60 * 60 * 24 * 90, "/");
        }

        string? currentSessionTicket = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _sessionCookieName);
        TResponseModel<DialogWebChatModelDB> initSessionTicket = await WebChatRepo.InitWebChatSessionAsync(new()
        {
            SessionTicket = currentSessionTicket,
            UserIdentityId = CurrentUserSession?.UserId,
            UserAgent = UserAgent.UserAgent,
            Language = UserAgent.Language,
            BaseUri = NavRepo.BaseUri,
        });
        dialogSession = initSessionTicket.Response;
        dialogSessionEdit = GlobalTools.CreateDeepCopy(dialogSession);

        if (initSessionTicket.Response is null)
        {
            SnackBarRepo.Error("initSessionTicket.Response is null");
            return;
        }

        await JsRuntime.InvokeVoidAsync("methods.CreateCookie", _sessionCookieName, initSessionTicket.Response.SessionTicketId, (initSessionTicket.Response.DeadlineUTC - DateTime.UtcNow).TotalSeconds, "/");
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await InitSession();
        if (dialogSession is not null)
        {
            await NewMessageWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.NewMessageWebChatNotifyReceive, dialogSession.Id.ToString()), NewMessageWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, propertiesValues: [new(Routes.DIALOG_CONTROLLER_NAME, BitConverter.GetBytes(dialogSession.Id))]);
            await StateGetWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateGetWebChatNotifyReceive, dialogSession.Id.ToString()), GetStateWebChatWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
            await StateSetWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateSetWebChatNotifyReceive, dialogSession.Id.ToString()), SetStateWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
        }
    }

    async void SetStateWebChatHandler(StateWebChatModel req)
    {
        ChatDialogOpen = req.StateDialog;
        if (dialogSession is not null)
            await EventsWebChatsHandleRepo.StateEchoWebChatAsync(new()
            {
                StateDialog = ChatDialogOpen,
                DialogId = dialogSession.Id,
                UserIdentityId = CurrentUserSession?.UserId,
            });
        await InvokeAsync(StateHasChanged);
    }

    async void GetStateWebChatWebChatHandler(GetStateWebChatEventModel req)
    {
        try
        {
            UserAgent = await JsRuntime.InvokeAsync<AboutUserAgentModel?>("methods.AboutUserAgent");
        }
        catch (TaskCanceledException)
        {
            RealtimeCore.Dispose();
            Dispose();
            return;
        }
        catch (OperationCanceledException)
        {
            RealtimeCore.Dispose();
            Dispose();
            return;
        }

        await EventsWebChatsHandleRepo.StateEchoWebChatAsync(new StateWebChatModel()
        {
            StateDialog = ChatDialogOpen,
            DialogId = req.DialogId,
            UserIdentityId = CurrentUserSession?.UserId,
        });
    }

    async void NewMessageWebChatHandler(NewMessageWebChatEventModel req)
    {
        List<Task> tasks = [];
        if (virtualizeComponent is not null)
            tasks.Add(virtualizeComponent.RefreshDataAsync());

        if (!ChatDialogOpen)
            missingMessages++;
        else
            missingMessages = 0;

        if (!ChatDialogOpen)
        {
            tasks.Add(Task.Run(async () => await JsRuntime.InvokeVoidAsync("effects.JQuery", "pulsate", "missingMessagesBadge")));
            tasks.Add(Task.Run(async () => await JsRuntime.InvokeVoidAsync("effects.Toast", "Новое сообщение в чате", req.TextMessage, "info", true, "#9EC600")));
        }

        if (!muteSound)
            tasks.Add(Task.Run(async () => await JsRuntime.InvokeVoidAsync("methods.PlayAudio", "audioPlayerChatWrapperComponent")));
        muteSound = false;

        await Task.WhenAll(tasks);
        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        NewMessageWebChatEventRepo.UnregisterAction();
        StateGetWebChatEventRepo.UnregisterAction(isMute: true);
        StateSetWebChatEventRepo.UnregisterAction(isMute: true);
    }

    void ShowHiddenInfo()
    {
        if (_selectedMessage is not null)
        {
            SnackBarRepo.Add($"Hidden information for ``", Severity.Info);
        }
    }

    void BanUser()
    {
        if (_selectedMessage is not null)
        {
            SnackBarRepo.Add($"`` has been banned!", Severity.Error);
        }
    }

    async Task RightClickMessage(MouseEventArgs args, MessageWebChatModelDB message)
    {
        _selectedMessage = message;
        if (_contextMenu != null)
            await _contextMenu.OpenMenuAsync(args);
    }

    async Task ClickMessage(MouseEventArgs args, MessageWebChatModelDB message)
    {
        _selectedMessage = message;
        SnackBarRepo.Add("Message clicked: " + message.Text, Severity.Info);
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void UsersEcho(List<PongClientsWebChatEventModel> usersEcho)
    {
        lock (UsersSessions)
        {
            UsersSessions.Clear();
            UsersSessions.AddRange(usersEcho);
        }
        InvokeAsync(StateHasChangedCall);
    }
}