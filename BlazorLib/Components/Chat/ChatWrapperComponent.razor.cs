////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Chat;

public partial class ChatWrapperComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IScrollListenerFactory ScrollListenerFactory { get; set; } = default!;

    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;


    ElementReference myTextArea;
    bool ChatDialogOpen;
    void ShowToggle()
    {
        ChatDialogOpen = !ChatDialogOpen;
    }

    List<MessageWebChatModel> messages = [];
    MessageWebChatModel? _selectedMessage;
    MudMenu? _contextMenu;
    IScrollListener? _scrollListener;
    bool Hovering => _bubbleHovering;
    bool _bubbleHovering;
    string? _textSendMessage;
    MessageWebChatModel? _hoverMessage;


    async Task SendMessage(MouseEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(_textSendMessage))
            return;

        await SetBusyAsync();
        messages.Add(new("Name", "OK", _textSendMessage.Trim(), DateTime.Now));
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
            messages.Add(new("Name", "OK", _textSendMessage.Trim(), DateTime.Now));
            _textSendMessage = null;
            await SetBusyAsync(false);
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        _scrollListener = ScrollListenerFactory.Create(null);
        _scrollListener.OnScroll += OnScrollAsync;

        messages.Add(new MessageWebChatModel("Obi-Wan Kenobi", "OK", "You were my brother Anakin.", DateTime.Now.AddHours(-2)));
        messages.Add(new MessageWebChatModel("Obi-Wan Kenobi", "OK", "I loved you.", DateTime.Now.AddHours(-2).AddMinutes(23)));
        messages.Add(new MessageWebChatModel("Anakin Skywalker", "AS", "I'm sorry.", DateTime.Now.AddHours(-1)));
    }

    void OnScrollAsync(object? sender, ScrollEventArgs e)
    {

    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        if (_scrollListener != null)
            _scrollListener.OnScroll -= OnScrollAsync;
    }

    void ShowHiddenInfo()
    {
        if (_selectedMessage is not null)
        {
            SnackBarRepo.Add($"Hidden information for {_selectedMessage.Name}", Severity.Info);
        }
    }

    void BanUser()
    {
        if (_selectedMessage is not null)
        {
            SnackBarRepo.Add($"{_selectedMessage.Name} has been banned!", Severity.Error);
        }
    }

    async Task RightClickMessage(MouseEventArgs args, MessageWebChatModel message)
    {
        _selectedMessage = message;
        if (_contextMenu != null)
            await _contextMenu.OpenMenuAsync(args);
    }

    async Task ClickMessage(MouseEventArgs args, MessageWebChatModel message)
    {
        _selectedMessage = message;
        SnackBarRepo.Add("Message clicked: " + message.Text, Severity.Info);
        await Task.CompletedTask;
    }
}