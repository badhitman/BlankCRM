////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

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
public partial class ChatWrapperComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;


    bool ChatDialogOpen;
    void ShowToggle()
    {
        ChatDialogOpen = !ChatDialogOpen;
    }

    List<MessageWebChatModelDB> messages = [];
    MessageWebChatModelDB? _selectedMessage;
    MudMenu? _contextMenu;

    string? _textSendMessage;
    bool CannotSendMessage => string.IsNullOrWhiteSpace(_textSendMessage) || IsBusyProgress;


    async Task SendMessage(MouseEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage))
            return;

        await SetBusyAsync();
        messages.Add(new() { Text = _textSendMessage.Trim(), CreatedAtUTC = DateTime.Now });
        _textSendMessage = null;
        await SetBusyAsync(false);
    }

    async Task OnKeyPresHandler(KeyboardEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage))
            return;

        if (args.Key == "Enter" && !args.ShiftKey)
        {
            await SetBusyAsync();
            messages.Add(new() { Text = _textSendMessage.Trim(), CreatedAtUTC = DateTime.Now });
            _textSendMessage = null;
            await SetBusyAsync(false);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {//name, value, seconds, path
        await base.OnInitializedAsync();
        UserInfoModel? currentUser = CurrentUserSession;
        string _cn = Path.Combine(Routes.TICKET_CONTROLLER_NAME, Routes.SESSION_CONTROLLER_NAME);
        string? currentSessionTicket = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _cn);
        if (string.IsNullOrWhiteSpace(currentSessionTicket))
        {
            await JsRuntime.InvokeVoidAsync("methods.CreateCookie", _cn, Guid.NewGuid().ToString(), 60 * 60 * 24 * 14, "/");
            currentSessionTicket = await JsRuntime.InvokeAsync<string?>("methods.ReadCookie", _cn);
        }
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