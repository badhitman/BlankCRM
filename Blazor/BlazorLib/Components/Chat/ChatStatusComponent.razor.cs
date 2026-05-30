////////////////////////////////////////////////
// � https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Chat;

/// <summary>
/// ChatStatusComponent
/// </summary>
public partial class ChatStatusComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IEventNotifyReceive<StateWebChatModel> StateEchoWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<ConnectionCloseWebChatEventModel> ConnectionCloseWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<ConnectionOpenWebChatEventModel> ConnectionOpenWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventsWebChatsNotifies EventsWebChatsHandleRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public int DialogId { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public required Action? OnLoadHandler { get; set; }


    readonly string LayoutContainerId = Guid.NewGuid().ToString();
    /// <inheritdoc/>
    public StateWebChatModel? StateWebChat { get; private set; }

    /// <inheritdoc/>
    public UserInfoModel? UserInfo { get; private set; }

    /// <inheritdoc/>
    public bool IsConnected { get; private set; }

    async void ConnectionCloseWebChatEventHandle(ConnectionCloseWebChatEventModel req)
    {
        UserInfo = null;
        StateWebChat = null;
        IsConnected = false;
        await InvokeAsync(StateHasChanged);
        if (OnLoadHandler is not null)
            OnLoadHandler();
    }
    async void ConnectionOpenWebChatEventHandle(ConnectionOpenWebChatEventModel req)
    {
        UserInfo = req.UserInfo;
        IsConnected = true;
        await InvokeAsync(StateHasChanged);
        if (OnLoadHandler is not null)
            OnLoadHandler();
    }
    async void StateEchoWebChatEventHandle(StateWebChatModel req)
    {
        StateWebChat = req;
        await InvokeAsync(StateHasChanged);
        if (OnLoadHandler is not null)
            OnLoadHandler();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await StateEchoWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.StateEchoWebChatNotifyReceive, DialogId.ToString()), StateEchoWebChatEventHandle, LayoutContainerId, null, isMute: true);
        await ConnectionOpenWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatNotifyReceive, DialogId.ToString()), ConnectionOpenWebChatEventHandle, LayoutContainerId, null, isMute: true);
        await ConnectionCloseWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatNotifyReceive, DialogId.ToString()), ConnectionCloseWebChatEventHandle, LayoutContainerId, null, isMute: true);
        //
        await EventsWebChatsHandleRepo.StateGetWebChatAsync(new() { DialogId = DialogId });
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        StateEchoWebChatEventRepo.UnregisterAction(isMute: true);
        ConnectionCloseWebChatEventRepo.UnregisterAction(isMute: true);
        ConnectionOpenWebChatEventRepo.UnregisterAction(isMute: true);
        base.Dispose();
    }
}