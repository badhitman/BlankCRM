////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using MudBlazor;
using Microsoft.Extensions.Options;
using static MudBlazor.CategoryTypes;

namespace BlazorLib.Components.ToolsApp;

/// <summary>
/// RowCommandComponent
/// </summary>
public partial class RowCommandComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IClientHTTPRestService ToolsExtRepo { get; set; } = default!;

    [Inject]
    ApiRestConfigModelDB ApiConnect { get; set; } = default!;

    [Inject]
    IServerToolsService ToolsLocalRepo { get; set; } = default!;

    [Inject]
    IToolsAppManager AppManagerRepo { get; set; } = default!;

    [Inject]
    IDialogService DialogRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ExeCommandModelDB CurrentCommand { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action ReloadCommandsHandle { get; set; }


    ExeCommandModelDB OriginCommand = default!;

    async Task RunCommand()
    {
        await SetBusyAsync();
        TResponseModel<string> res = await ToolsExtRepo.ExeCommandAsync(CurrentCommand);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        DialogParameters<ResultExeCommandComponent> parameters = new()
        {
            { x => x.ShellOutput, res.Response },
            { x => x.CurrentCommand, OriginCommand },
        };

        DialogOptions options = new()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
        };

        await DialogRepo.ShowAsync<ResultExeCommandComponent>("Shell/cmd", parameters, options);
    }

    void CancelEdit()
    {
        CurrentCommand = GlobalTools.CreateDeepCopy(OriginCommand)!;
    }

    async Task SaveRow()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await AppManagerRepo.UpdateOrCreateExeCommandAsync(CurrentCommand);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        OriginCommand = GlobalTools.CreateDeepCopy(CurrentCommand)!;
        await SetBusyAsync(false);
        ReloadCommandsHandle();

    }

    async Task DeleteCommand()
    {
        await SetBusyAsync();
        ResponseBaseModel res = await AppManagerRepo.DeleteExeCommandAsync(CurrentCommand.Id);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
        ReloadCommandsHandle();
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        OriginCommand = GlobalTools.CreateDeepCopy(CurrentCommand)!;
    }

    bool IsEdited =>
        OriginCommand.Name != CurrentCommand.Name ||
        OriginCommand.FileName != CurrentCommand.FileName ||
        OriginCommand.Arguments != CurrentCommand.Arguments
        ;
}