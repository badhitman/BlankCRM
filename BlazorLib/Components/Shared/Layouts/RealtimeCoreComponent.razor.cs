////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using BlazorLib.Components.Chat;
using SharedLib;

namespace BlazorLib.Components.Shared.Layouts;

/// <summary>
/// RealtimeCoreComponent
/// </summary>
public partial class RealtimeCoreComponent
{
    [Inject]
    NavigationManager NavRepo { get; set; } = default!;

    [Inject]
    IOptions<ServerConfigModel> WebConfig { get; set; } = default!;

    [Inject]
    IEventsWebChatsNotifies NotifyWebChatRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<PingClientsWebChatEventModel> PingClientsWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<PongClientsWebChatEventModel> PongClientsWebChatEventRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<ConnectOpenWebChatEventModel> ConnectionOpenWebChatRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<ConnectCloseWebChatEventModel> ConnectionCloseWebChatRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required NavMainMenuModel NavMainMenu { get; set; }


    ChatWrapperComponent? chatWrapperRef;
    readonly List<PongClientsWebChatEventModel> UsersEcho = [];

    /// <inheritdoc/>
    public string LayoutContainerId { get; private set; } = Guid.NewGuid().ToString();

    string GetRootDomain()
    {
        Uri _uri = new(NavRepo.BaseUri);
        return $"{_uri.Scheme}://api.{_uri.Authority}/swagger/";
    }

    /// <inheritdoc/>
    public async Task PingAllUsers()
    {
        lock (UsersEcho)
        {
            UsersEcho.Clear();
        }
        chatWrapperRef?.UsersEcho(UsersEcho);
        await NotifyWebChatRepo.PingClientsWebChatAsync(new() { LayoutContainerId = LayoutContainerId });
    }

    async void PingClientsWebChatHandler(PingClientsWebChatEventModel req)
    {
        if (req.LayoutContainerId.Equals(LayoutContainerId))
            return;

        await NotifyWebChatRepo.PongClientWebChatAsync(new PongClientsWebChatEventModel()
        {
            CurrentUserSession = CurrentUserSession,
            ResponseContainerGUID = req.LayoutContainerId,
            SenderContainerGUID = LayoutContainerId,
            StateDialog = chatWrapperRef?.ChatDialogOpen == true
        });
    }

    void PongClientsWebChatHandler(PongClientsWebChatEventModel model)
    {
        lock (UsersEcho)
        {
            UsersEcho.Add(model);
        }
        chatWrapperRef?.UsersEcho(UsersEcho);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await PongClientsWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.PongClientWebChatNotifyReceive, LayoutContainerId.ToString()).Replace("\\", "/"), PongClientsWebChatHandler, CurrentUserSessionBytes(LayoutContainerId), isMute: true);
        await ConnectionCloseWebChatRepo.RegisterAction(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatNotifyReceive.Replace("\\", "/"), ConnectionCloseWebChatHandler, CurrentUserSessionBytes(LayoutContainerId), isMute: true);
        await PingClientsWebChatEventRepo.RegisterAction(GlobalStaticConstantsTransmission.TransmissionQueues.PingClientsWebChatNotifyReceive.Replace("\\", "/"), PingClientsWebChatHandler, CurrentUserSessionBytes(LayoutContainerId), isMute: true);
        await ConnectionOpenWebChatRepo.RegisterAction(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatNotifyReceive.Replace("\\", "/"), ConnectionOpenWebChatHandler, CurrentUserSessionBytes(LayoutContainerId));
    }

    async void ConnectionCloseWebChatHandler(ConnectCloseWebChatEventModel model)
    {
        await PingAllUsers();
    }

    async void ConnectionOpenWebChatHandler(ConnectOpenWebChatEventModel model)
    {
        await PingAllUsers();
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        PingClientsWebChatEventRepo.UnregisterAction(isMute: true);
        PongClientsWebChatEventRepo.UnregisterAction(isMute: true);
        ConnectionCloseWebChatRepo.UnregisterAction(isMute: true);
        ConnectionOpenWebChatRepo.UnregisterAction();
        base.Dispose();
    }
}