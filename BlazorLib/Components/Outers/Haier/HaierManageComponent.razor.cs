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
    IFeedsHaierProffRuService haierproffRepo { get; set; } = default!;


    async Task Download()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await haierproffRepo.DownloadAndSaveAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }
}