﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.ToolsApp;

/// <summary>
/// SyncDirectoryDialogComponent
/// </summary>
public partial class SyncDirectoryDialogComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ApiRestConfigModelDB ApiConnect { get; set; } = default!;

    [Inject]
    IToolsAppManager ToolsApp { get; set; } = default!;

    [Inject]
    IClientHTTPRestService RestClientRepo { get; set; } = default!;


    /// <summary>
    /// SyncRuleId
    /// </summary>
    [Parameter, EditorRequired]
    public int SyncRuleId { get; set; }

    /// <summary>
    /// MudDialog
    /// </summary>
    [CascadingParameter, EditorRequired]
    public required IMudDialogInstance MudDialog { get; set; }


    string? Name { get; set; }
    string? LocalDirectory { get; set; }
    string? RemoteDirectory { get; set; }

    SyncDirectoryModelDB? SyncDir;

    DirectoryInfo? LocalDirectoryInfo;
    ResponseBaseModel? RemoteDirectoryInfo;

    bool ValidateForm => !string.IsNullOrWhiteSpace(LocalDirectory) && new DirectoryInfo(LocalDirectory).Exists;

    bool IsEquals => SyncDir is not null &&
        (Name == SyncDir.Name || (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(SyncDir.Name))) &&
        (LocalDirectory == SyncDir.LocalDirectory || (string.IsNullOrWhiteSpace(LocalDirectory) && string.IsNullOrWhiteSpace(SyncDir.LocalDirectory))) &&
        (RemoteDirectory == SyncDir.RemoteDirectory || (string.IsNullOrWhiteSpace(RemoteDirectory) && string.IsNullOrWhiteSpace(SyncDir.RemoteDirectory)));

    bool initDelete;

    async Task DeleteSyncRule()
    {
        if (SyncDir is null)
        {
            SnackbarRepo.Error("SyncDir is null");
            return;
        }
        if (SyncDir.Id == 0)
        {
            SnackbarRepo.Error("SyncDir.Id == 0");
            return;
        }

        if (!initDelete)
        {
            initDelete = true;
            return;
        }
        await SetBusy();
        ResponseBaseModel res = await ToolsApp.DeleteSyncDirectory(SyncDir.Id);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusy(false);
        MudDialog.Close(DialogResult.Ok(true));
    }

    async Task SaveSyncDirectoryRule()
    {
        SyncDirectoryModelDB req = new()
        {
            LocalDirectory = LocalDirectory,
            Name = Name ?? string.Empty,
            RemoteDirectory = RemoteDirectory,
            ParentId = ApiConnect.Id,
        };
        await SetBusy();
        ResponseBaseModel res = await ToolsApp.UpdateOrCreateSyncDirectory(req);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusy(false);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    async Task Test(bool testForm = false)
    {
        if (SyncDir is null)
        {
            SnackbarRepo.Error("SyncDir is null");
            return;
        }

        await SetBusy();
        SyncDirectoryModelDB? backupDir = null;
        if (testForm)
        {
            backupDir = GlobalTools.CreateDeepCopy(SyncDir);

            SyncDir.LocalDirectory = LocalDirectory;
            SyncDir.RemoteDirectory = RemoteDirectory;
        }

        LocalDirectoryInfo = string.IsNullOrWhiteSpace(SyncDir?.LocalDirectory)
            ? null
            : new(SyncDir.LocalDirectory);

        if (LocalDirectoryInfo is null)
            SnackbarRepo.Error("Не указана локальная папка");
        else if (!LocalDirectoryInfo.Exists)
            SnackbarRepo.Error("Локальной папки не существует");
        else
            SnackbarRepo.Success("Локальная папка существует");

        RemoteDirectoryInfo = string.IsNullOrWhiteSpace(SyncDir?.RemoteDirectory)
                ? null
                : await RestClientRepo.DirectoryExist(SyncDir.RemoteDirectory);

        if (RemoteDirectoryInfo is not null)
            SnackbarRepo.ShowMessagesResponse(RemoteDirectoryInfo.Messages);
        else
            SnackbarRepo.Error("Не указана директория на удалённом сервере");

        if (backupDir is not null)
            SyncDir?.Update(backupDir);

        await SetBusy(false);
    }

    void ResetValues()
    {
        Name = SyncDir?.Name;
        LocalDirectory = SyncDir?.LocalDirectory;
        RemoteDirectory = SyncDir?.RemoteDirectory;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (SyncRuleId != 0)
        {
            await SetBusy();
            SyncDir = await ToolsApp.ReadSyncDirectory(SyncRuleId);
            await SetBusy(false);
        }
        else
            SyncDir = SyncDirectoryModelDB.BuildEmpty(ApiConnect.Id);

        ResetValues();
    }
}