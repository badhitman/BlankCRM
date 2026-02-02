////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using static SharedLib.GlobalStaticConstantsRoutes;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SharedLib;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace BlazorLib.Components.Chat;

/// <summary>
/// ChatWrapperComponent
/// </summary>
public partial class ChatWrapperComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;

    [Inject]
    IWebChatService WebChatRepo { get; set; } = default!;


    MessageWebChatModelDB? _selectedMessage;
    MudMenu? _contextMenu;
    InitWebChatSessionResponseModel? ticketSession;
    Virtualize<MessageWebChatModelDB>? virtualizeComponent;
    string? _textSendMessage;
    bool CannotSendMessage => string.IsNullOrWhiteSpace(_textSendMessage) || IsBusyProgress;
    static readonly int virtualCacheSize = 50;

    bool ChatDialogOpen;
    void ShowToggle()
    {
        ChatDialogOpen = !ChatDialogOpen;
    }

    async ValueTask<ItemsProviderResult<MessageWebChatModelDB>> LoadMessages(ItemsProviderRequest request)
    {
        if (ticketSession is null)
            return new ItemsProviderResult<MessageWebChatModelDB>([], 0);

        SelectMessagesForWebChatRequestModel req = new()
        {
            SessionTicketId = ticketSession.SessionTicket,
            StartIndex = request.StartIndex,
            Count = request.Count,
        };
        TResponseModel<SelectMessagesForWebChatResponseModel> res = await WebChatRepo.SelectMessagesWebChatAsync(req, request.CancellationToken);

        if (res.Response is null)
            return new ItemsProviderResult<MessageWebChatModelDB>([], 0);

        //List<KeyValuePair<string?, List<MessageWebChatModelDB>>> chatSrc = [];
        ////KeyValuePair<string?, List<MessageWebChatModelDB>> _nr = new();

        //foreach(MessageWebChatModelDB _msg in res.Response)
        //{
        //    if (chatSrc.Count == 0 || chatSrc.Last().Key != _msg.SenderUserIdentityId)
        //        chatSrc.Add(new(_msg.SenderUserIdentityId, [_msg]));
        //    else
        //        chatSrc.Last().Value.Add(_msg);
        //}

        return new ItemsProviderResult<MessageWebChatModelDB>(res.Response.Messages, res.Response.TotalRowsCount);
    }


    async Task SendMessage(MouseEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage) || ticketSession is null)
            return;

        MessageWebChatModelDB req = new()
        {
            Text = _textSendMessage.Trim(),
            SenderUserIdentityId = CurrentUserSession?.UserId,
            DialogOwnerId = ticketSession.DialogId
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
                DialogOwnerId = ticketSession.DialogId
            };
            await SetBusyAsync();
            await WebChatRepo.CreateMessageWebChatAsync(req);
            _textSendMessage = null;
            if (virtualizeComponent is not null)
                await virtualizeComponent.RefreshDataAsync();
            await SetBusyAsync(false);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        UserInfoModel? currentUser = CurrentUserSession;
        string _cn = Path.Combine(Routes.TICKET_CONTROLLER_NAME, Routes.SESSION_CONTROLLER_NAME);
        string? currentSessionTicket = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _cn);
        TResponseModel<InitWebChatSessionResponseModel> initSessionTicket = await WebChatRepo.InitWebChatSessionAsync(new() { SessionTicket = currentSessionTicket, UserIdentityId = currentUser?.UserId });

        if (currentSessionTicket != initSessionTicket.Response?.SessionTicket)
            await JsRuntime.InvokeVoidAsync("methods.CreateCookie", _cn, initSessionTicket.Response?.SessionTicket, GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds, "/");
        else
            await JsRuntime.InvokeVoidAsync("methods.UpdateCookie", _cn, initSessionTicket.Response?.SessionTicket, GlobalToolsStandard.WebChatTicketSessionDeadlineSeconds, "/");

        ticketSession = initSessionTicket.Response;
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