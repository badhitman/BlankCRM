////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components.Web.Virtualization;
using static SharedLib.GlobalStaticConstantsRoutes;
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


    MessageWebChatModelDB? _selectedMessage;
    MudMenu? _contextMenu;
    DialogWebChatModelDB? ticketSession, ticketSessionEdit;
    string? lastUserId;
    Virtualize<MessageWebChatModelDB>? virtualizeComponent;
    string? _textSendMessage;
    bool CannotSendMessage => string.IsNullOrWhiteSpace(_textSendMessage) || IsBusyProgress;
    static readonly int virtualCacheSize = 50;

    bool ChatDialogOpen;
    async Task ShowToggle()
    {
        ChatDialogOpen = !ChatDialogOpen;

        if (ChatDialogOpen)
            await InitSession();
    }

    async ValueTask<ItemsProviderResult<MessageWebChatModelDB>> LoadMessages(ItemsProviderRequest request)
    {
        if (ticketSession is null)
            return new ItemsProviderResult<MessageWebChatModelDB>([], 0);

        SelectMessagesForWebChatRequestModel req = new()
        {
            SessionTicketId = ticketSession.SessionTicketId,
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
        if (ticketSessionEdit is null)
            return;

        await SetBusyAsync();
        ResponseBaseModel res = await WebChatRepo.UpdateDialogWebChatInitiatorAsync(new()
        {
            SenderActionUserId = CurrentUserSession?.UserId,
            Payload = ticketSessionEdit
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await InitSession();

        await SetBusyAsync(false);
    }

    async Task SendMessage(MouseEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage) || ticketSession is null)
            return;

        MessageWebChatModelDB req = new()
        {
            Text = _textSendMessage.Trim(),
            SenderUserIdentityId = CurrentUserSession?.UserId,
            DialogOwnerId = ticketSession.Id,
            InitiatorMessageSender = true,
        };

        await SetBusyAsync();

        await WebChatRepo.CreateMessageWebChatAsync(req);
        _textSendMessage = null;

        if (virtualizeComponent is not null)
            await virtualizeComponent.RefreshDataAsync();

        await SetBusyAsync(false);
    }

    async Task OnKeyPresHandler(KeyboardEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage) || ticketSession is null)
            return;

        if (args.Key == "Enter" && !args.ShiftKey)
        {
            MessageWebChatModelDB req = new()
            {
                Text = _textSendMessage.Trim(),
                SenderUserIdentityId = CurrentUserSession?.UserId,
                DialogOwnerId = ticketSession.Id,
                InitiatorMessageSender = true,
            };
            await SetBusyAsync();
            await WebChatRepo.CreateMessageWebChatAsync(req);
            _textSendMessage = null;
            if (virtualizeComponent is not null)
                await virtualizeComponent.RefreshDataAsync();
            await SetBusyAsync(false);
        }
    }

    async Task InitSession()
    {
        string
            _sessionCookieName = Path.Combine(Routes.TICKET_CONTROLLER_NAME, Routes.SESSION_CONTROLLER_NAME).Replace("\\", "/"),
            _lastUserIdCookieName = Path.Combine(_sessionCookieName, $"{Routes.USER_CONTROLLER_NAME}-{Routes.IDENTITY_CONTROLLER_NAME}").Replace("\\", "/");

        lastUserId = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _lastUserIdCookieName);
        //SnackBarRepo.Info($"");
        if (!string.IsNullOrWhiteSpace(CurrentUserSession?.UserId))
        {
            if (lastUserId != _lastUserIdCookieName)
            {
                await JsRuntime.InvokeVoidAsync("methods.CreateCookie", _lastUserIdCookieName, CurrentUserSession.UserId, 60 * 60 * 24 * 90, "/");
                //SnackBarRepo.Info($"");
            }
        }

        string? currentSessionTicket = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _sessionCookieName);
        //SnackBarRepo.Info($"");
        TResponseModel<DialogWebChatModelDB> initSessionTicket = await WebChatRepo.InitWebChatSessionAsync(new()
        {
            SessionTicket = currentSessionTicket,
            UserIdentityId = CurrentUserSession?.UserId
        });
        ticketSession = initSessionTicket.Response;
        ticketSessionEdit = GlobalTools.CreateDeepCopy(ticketSession);

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
    }

    /// <inheritdoc/>
    public override void Dispose()
    {

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
}