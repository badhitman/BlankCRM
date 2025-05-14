////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// DashboardStockSharpComponent
/// </summary>
public partial class DashboardStockSharpComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IDriverStockSharpService driverRepo { get; set; } = default!;

    AboutConnectResponseModel? AboutConnection;

    async Task Connect()
    {
        await SetBusyAsync();
        ResponseBaseModel _con = await driverRepo.Connect();
        SnackbarRepo.ShowMessagesResponse(_con.Messages);
        await GetStatusConnection();
    }

    async Task Disconnect()
    {
        await SetBusyAsync();
        ResponseBaseModel _con = await driverRepo.Disconnect();
        SnackbarRepo.ShowMessagesResponse(_con.Messages);
        await GetStatusConnection();
    }

    async Task GetStatusConnection()
    {
        if (!IsBusyProgress)
            await SetBusyAsync();
        AboutConnection = await driverRepo.AboutConnection();
        SnackbarRepo.ShowMessagesResponse(AboutConnection.Messages);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        await GetStatusConnection();
    }
}