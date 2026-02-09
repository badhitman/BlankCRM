////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using static MudBlazor.CategoryTypes;

namespace BlazorLib.Components.mqtt;

/// <summary>
/// ClientsListMqttComponent
/// </summary>
public partial class ClientsListMqttComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IWebChatService WebChatsRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<ConnectionOpenWebChatEventModel> ConnectionOpenWebChatRepo { get; set; } = default!;

    [Inject]
    IEventNotifyReceive<ConnectionCloseWebChatEventModel> ConnectionCloseWebChatRepo { get; set; } = default!;


    /// <inheritdoc/>
    public string LayoutContainerId { get; private set; } = Guid.NewGuid().ToString();

    List<MqttClientModel> clients = [];
    string searchString1 = "";
    bool FilterFunc1(MqttClientModel element) => FilterFunc(element, searchString1);

    static bool FilterFunc(MqttClientModel element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.RemoteEndPoint?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;
        if (element.Id?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        return false;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ConnectionCloseWebChatRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionCloseWebChatNotifyReceive, "#"), ConnectionCloseWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
        await ConnectionOpenWebChatRepo.RegisterAction(Path.Combine(GlobalStaticConstantsTransmission.TransmissionQueues.ConnectionOpenWebChatNotifyReceive, "#"), ConnectionOpenWebChatHandler, LayoutContainerId, CurrentUserSessionBytes, isMute: true);
        await Reload();
    }

    async void ConnectionCloseWebChatHandler(ConnectionCloseWebChatEventModel model)
    {
        await Reload();
        await InvokeAsync(StateHasChanged);
    }

    async void ConnectionOpenWebChatHandler(ConnectionOpenWebChatEventModel model)
    {
        await Reload();
        await InvokeAsync(StateHasChanged);
    }

    async Task Reload()
    {
        await SetBusyAsync();
        TResponseModel<List<MqttClientModel>> res = await WebChatsRepo.GetClientsConnectionsAsync(new GetClientsRequestModel() { });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        lock (clients)
        {
            clients.Clear();
            if (res.Response is not null && res.Response.Count != 0)
                clients = res.Response;
        }
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        ConnectionCloseWebChatRepo.UnregisterAction(isMute: true);
        ConnectionOpenWebChatRepo.UnregisterAction(isMute: true);
        base.Dispose();
    }
}