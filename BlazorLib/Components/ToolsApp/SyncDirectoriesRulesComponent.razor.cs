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
    IToolsAppManager AppManagerRepo { get; set; } = default!;

    [Inject]
    IDialogService DialogService { get; set; } = default!;


    SyncDirectoryModelDB[] SyncDirectories { get; set; } = [];

    SyncDirectoryModelDB? SelectedSyncDir { get; set; }

    async void CloseCommandAction()
    {
        SelectedSyncDir = null;
        await SetBusy();
        await ReloadDirectories();
        await SetBusy(false);
    }

    async Task OpenSyncRule(int ruleId = 0)
    {
        DialogOptions options = new() { BackgroundClass = "my-custom-class" };
        DialogParameters<SyncDirectoryDialogComponent> parameters = new()
        {
            { x => x.SyncRuleId, ruleId },
        };

        IDialogReference dialog = await DialogService.ShowAsync<SyncDirectoryDialogComponent>("Синхронизация папок/файлов", parameters, options);
        DialogResult? res = await  dialog.Result;
        await ReloadDirectories();
    }

    async Task ReloadDirectories()
    {
        await SetBusy();
        SyncDirectories = await AppManagerRepo.GetSyncDirectoriesForConfigAsync(ApiConnect.Id);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadDirectories();
    }
}