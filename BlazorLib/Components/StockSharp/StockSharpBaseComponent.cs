////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// StockSharpBaseComponent
/// </summary>
public class StockSharpBaseComponent : BlazorBusyComponentBaseModel
{
    /// <inheritdoc/>
    [Inject]
    protected IDriverStockSharpService DriverRepo { get; set; } = default!;


    /// <inheritdoc/>
    protected AboutConnectResponseModel? AboutConnection;

    /// <inheritdoc/>
    protected bool EachDisable => AboutConnection is null || AboutConnection.ConnectionState != ConnectionStatesEnum.Connected;


    /// <inheritdoc/>
    protected async Task Connect(ConnectRequestModel req)
    {
        await SetBusyAsync();
        ResponseBaseModel _con = await DriverRepo.Connect(req);
        SnackbarRepo.ShowMessagesResponse(_con.Messages);
        await GetStatusConnection();
    }

    /// <inheritdoc/>
    protected async Task Disconnect()
    {
        await SetBusyAsync();
        ResponseBaseModel _con = await DriverRepo.Disconnect();
        SnackbarRepo.ShowMessagesResponse(_con.Messages);
        //_con = await DriverRepo.Terminate();
        //SnackbarRepo.ShowMessagesResponse(_con.Messages);
        await GetStatusConnection();
    }

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
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        await GetStatusConnection();
    }
}