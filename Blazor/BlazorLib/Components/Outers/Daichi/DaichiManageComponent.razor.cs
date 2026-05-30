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
    IDaichiBusinessApiTransmission daichiRepo { get; set; } = default!;


    static string[] ApplicationsFilterSet = ["ApiDaichiBusiness"];

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
        SnackBarRepo.Info("Задание отправлено в очередь");
        await Task.Delay(5000);
        HealthCheck = await daichiRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }
}