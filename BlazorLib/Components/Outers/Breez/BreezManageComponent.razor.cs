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
    IBreezRuApiTransmission breezRepo { get; set; } = default!;


    TResponseModel<List<RabbitMqManagementResponseModel>>? HealthCheck;
    static string[] ApplicationsFilterSet = ["ApiBreezRu"];

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
        SnackbarRepo.Info("Задание отправлено в очередь");
        await Task.Delay(5000);
        HealthCheck = await breezRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }
}