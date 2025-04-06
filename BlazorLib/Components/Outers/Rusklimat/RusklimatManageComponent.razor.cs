﻿////////////////////////////////////////////////
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


    async Task Download()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await rusklimatRepo.DownloadAndSaveAsync();
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }
}