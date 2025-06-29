////////////////////////////////////////////////
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
    IToolsAppManager AppManagerRepo { get; set; } = default!;

    [Inject]
    IClientRestToolsService RestClientRepo { get; set; } = default!;


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
            SnackBarRepo.Error("SyncDir is null");
            return;
        }
        if (SyncDir.Id == 0)
        {
            SnackBarRepo.Error("SyncDir.Id == 0");
            return;
        }

        if (!initDelete)
        {
            initDelete = true;
            return;
        }
        await SetBusyAsync();
        ResponseBaseModel res = await AppManagerRepo.DeleteSyncDirectoryAsync(SyncDir.Id);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
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
            Id = SyncDir?.Id ?? 0
        };
        await SetBusyAsync();
        ResponseBaseModel res = await AppManagerRepo.UpdateOrCreateSyncDirectoryAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    async Task Test(bool testForm = false)
    {
        if (SyncDir is null)
        {
            SnackBarRepo.Error("SyncDir is null");
            return;
        }

        await SetBusyAsync();
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
            SnackBarRepo.Error("Не указана локальная папка");
        else if (!LocalDirectoryInfo.Exists)
            SnackBarRepo.Error("Локальной папки не существует");
        else
            SnackBarRepo.Success("Локальная папка существует");

        RemoteDirectoryInfo = string.IsNullOrWhiteSpace(SyncDir?.RemoteDirectory)
                ? null
                : await RestClientRepo.DirectoryExistAsync(SyncDir.RemoteDirectory);

        if (RemoteDirectoryInfo is not null)
            SnackBarRepo.ShowMessagesResponse(RemoteDirectoryInfo.Messages);
        else
            SnackBarRepo.Error("Не указана директория на удалённом сервере");

        if (backupDir is not null)
            SyncDir?.Update(backupDir);

        await SetBusyAsync(false);
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
            await SetBusyAsync();
            SyncDir = await AppManagerRepo.ReadSyncDirectoryAsync(SyncRuleId);
            await SetBusyAsync(false);
        }
        else
            SyncDir = SyncDirectoryModelDB.BuildEmpty(ApiConnect.Id);

        ResetValues();
    }
}