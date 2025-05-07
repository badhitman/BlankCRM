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

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        AboutConnection = await driverRepo.AboutConnection();
        SnackbarRepo.ShowMessagesResponse(AboutConnection.Messages);
        await SetBusyAsync(false);
    }
}