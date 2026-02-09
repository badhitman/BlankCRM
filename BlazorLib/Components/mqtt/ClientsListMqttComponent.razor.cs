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


    List<MqttClientModel>? clients;
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