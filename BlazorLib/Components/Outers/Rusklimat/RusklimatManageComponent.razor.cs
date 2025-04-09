////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Outers.Rusklimat;

/// <summary>
/// RusklimatManageComponent
/// </summary>
public partial class RusklimatManageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRusklimatComApiService rusklimatRepo { get; set; } = default!;


    TResponseModel<List<RabbitMqManagementResponseModel>>? HealthCheck;

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        HealthCheck = await rusklimatRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }

    async Task Download()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await rusklimatRepo.DownloadAndSaveAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        HealthCheck = await rusklimatRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }
}