////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace StockSharpMauiApp.Components.Pages;

public partial class Home : BlazorBusyComponentBaseModel
{
    [Inject]
    IStockSharpDriverService ssRepo { get; set; } = default!;


    async Task Click1()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await ssRepo.PingAsync();
        await SetBusyAsync(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
    }
}