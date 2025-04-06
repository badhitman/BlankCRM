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
        var res = await daichiRepo.DownloadAndSaveAsync();
    }
}