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


    async Task Download()
    {
        var res = await breezRepo.DownloadAndSaveAsync();
    }
}