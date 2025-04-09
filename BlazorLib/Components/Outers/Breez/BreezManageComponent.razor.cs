////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Outers.Breez;

/// <summary>
/// BreezManageComponent
/// </summary>
public partial class BreezManageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IBreezRuApiService breezRepo { get; set; } = default!;


    TResponseModel<List<RabbitMqManagementResponseModel>>? HealthCheck;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        HealthCheck = await breezRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }

    async Task Download()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await breezRepo.DownloadAndSaveAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        HealthCheck = await breezRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }
}