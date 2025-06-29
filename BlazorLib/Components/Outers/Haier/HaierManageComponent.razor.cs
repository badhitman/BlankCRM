////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Outers.Haier;

/// <summary>
/// HaierManageComponent
/// </summary>
public partial class HaierManageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IFeedsHaierProffRuService haierProffRepo { get; set; } = default!;


    TResponseModel<List<RabbitMqManagementResponseModel>>? HealthCheck;
    static string[] ApplicationsFilterSet = ["FeedsHaierProffRu"];

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        HealthCheck = await haierProffRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }

    async Task Download()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await haierProffRepo.DownloadAndSaveAsync();
        SnackBarRepo.Info("Задание отправлено в очередь");
        await Task.Delay(5000);
        HealthCheck = await haierProffRepo.HealthCheckAsync();
        await SetBusyAsync(false);
    }
}