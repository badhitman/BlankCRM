////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.mqtt;

public partial class ClientsListMqttComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IWebChatService WebChatsRepo { get; set; } = default!;

    List<MqttClientModel>? clients;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Reload();
    }

    async Task Reload()
    {
        await SetBusyAsync();
        TResponseModel<List<MqttClientModel>> res = await WebChatsRepo.GetClientsConnectionsAsync(new GetClientsRequestModel() { });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        clients = res.Response;
        await SetBusyAsync(false);
    }
}