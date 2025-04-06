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


    async Task Download()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await daichiRepo.DownloadAndSaveAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }
}