////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Outers.Daichi;

/// <summary>
/// DaichiManageComponent
/// </summary>
public partial class DaichiManageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDaichiBusinessApiService daichiRepo { get; set; } = default!;


    TResponseModel<List<RabbitMqManagementResponseModel>>? HealthCheck;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        HealthCheck = await daichiRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }

    async Task Download()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await daichiRepo.DownloadAndSaveAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        HealthCheck = await daichiRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }
}