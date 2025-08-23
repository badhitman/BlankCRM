////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// StockSharpAboutComponent
/// </summary>
public class StockSharpAboutComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected IDriverStockSharpService DriverRepo { get; set; } = default!;

    /// <inheritdoc/>
    protected AboutConnectResponseModel? AboutConnection;

    /// <inheritdoc/>
    public bool EachDisable => AboutConnection is null || AboutConnection.ConnectionState != ConnectionStatesEnum.Connected;


    /// <inheritdoc/>
    protected virtual async Task GetStatusConnection()
    {
        if (!IsBusyProgress)
            await SetBusyAsync();

        if (AboutConnection is null)
            AboutConnection = await DriverRepo.AboutConnection();
        else
            AboutConnection.Update(await DriverRepo.AboutConnection());

        await SetBusyAsync(false);
        SnackBarRepo.ShowMessagesResponse(AboutConnection.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        await GetStatusConnection();
    }
}