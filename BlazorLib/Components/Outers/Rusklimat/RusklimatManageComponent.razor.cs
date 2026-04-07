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
    IRusklimatComApiTransmission rusklimatRepo { get; set; } = default!;


    TResponseModel<List<RabbitMqManagementResponseModel>>? HealthCheck;
    static string[] ApplicationsFilterSet = ["ApiRusklimatCom"];

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
        SnackBarRepo.Info("Задание отправлено в очередь");
        await Task.Delay(5000);
        HealthCheck = await rusklimatRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }
}