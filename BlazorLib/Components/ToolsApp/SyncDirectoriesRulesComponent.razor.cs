////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.ToolsApp;

/// <summary>
/// SyncDirectoriesRulesComponent
/// </summary>
public partial class SyncDirectoriesRulesComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ApiRestConfigModelDB ApiConnect { get; set; } = default!;

    [Inject]
    IClientHTTPRestService RestClientRepo { get; set; } = default!;

    [Inject]
    IToolsAppManager ToolsApp { get; set; } = default!;

    [Inject]
    IDialogService DialogService { get; set; } = default!;


    SyncDirectoryModelDB[] SyncDirectories { get; set; } = default!;

    async Task CreateSyncRule()
    {
        DialogOptions options = new() { BackgroundClass = "my-custom-class" };

        /*
         var parameters = new DialogParameters<SyncDirectoryDialogComponent>
        {
            { x => x.ContentText, "Your computer seems very slow, click the download button to download free RAM." },
            { x => x.ButtonText, "Download" },
            { x => x.Color, Color.Info }
        };
         */

        _ = await DialogService.ShowAsync<SyncDirectoryDialogComponent>("Синхронизация папок/файлов", options);
        await ReloadDirectories();
    }

    async Task ReloadDirectories()
    {
        await SetBusy();
        SyncDirectories = await ToolsApp.GetSyncDirectoriesForConfig(ApiConnect.Id);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadDirectories();
    }
}