////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Chat;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using SharedLib;

namespace BlazorLib.Components.Shared.Layouts;

/// <summary>
/// RealtimeCoreComponent
/// </summary>
public partial class RealtimeCoreComponent
{
    [Inject]
    IJSRuntime JsRuntime { get; set; } = default!;

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
    IEventNotifyReceive<ConnectionOpenWebChatEventModel> ConnectionOpenWebChatRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<ConnectionCloseWebChatEventModel> ConnectionCloseWebChatRepo { get; set; } = default!;


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



        try
        {
            _ = await JsRuntime.InvokeAsync<AboutUserAgentModel?>("methods.AboutUserAgent", timeout: TimeSpan.FromSeconds(5));
        }
        catch (TaskCanceledException)
        {
            chatWrapperRef?.Dispose();
            Dispose();
            return;
        }
        catch (OperationCanceledException)
        {
            chatWrapperRef?.Dispose();
            Dispose();
            return;
        }


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
        await PingClientsWebChatEventRepo.RegisterAction(GlobalStaticConstantsTransmission.TransmissionQueues.PingClientsWebChatNotifyReceive, PingClientsWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
        await PongClientsWebChatEventRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.PongClientWebChatNotifyReceive, LayoutContainerId.ToString()), PongClientsWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
        await ConnectionCloseWebChatRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatNotifyReceive, "#"), ConnectionCloseWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
        await ConnectionOpenWebChatRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatNotifyReceive, "#"), ConnectionOpenWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: WebConfig.Value.WebChatEnable);
    }

    async void ConnectionCloseWebChatHandler(ConnectionCloseWebChatEventModel model)
    {
        await PingAllUsers();
    }

    async void ConnectionOpenWebChatHandler(ConnectionOpenWebChatEventModel model)
    {
        await PingAllUsers();
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        PingClientsWebChatEventRepo.UnregisterAction(isMute: true);
        PongClientsWebChatEventRepo.UnregisterAction(isMute: true);
        ConnectionCloseWebChatRepo.UnregisterAction(isMute: true);
        ConnectionOpenWebChatRepo.UnregisterAction(isMute: WebConfig.Value.WebChatEnable);
        base.Dispose();
    }
}