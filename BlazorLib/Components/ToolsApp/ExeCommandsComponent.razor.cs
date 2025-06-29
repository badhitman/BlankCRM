////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.ToolsApp;

/// <summary>
/// ExeCommandsComponent
/// </summary>
public partial class ExeCommandsComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    ApiRestConfigModelDB ApiConnect { get; set; } = default!;

    [Inject]
    IToolsAppManager AppManagerRepo { get; set; } = default!;


    ExeCommandModelDB[] ExeCommands { get; set; } = [];

    private ExeCommandModelDB newCommand = default!;

    async void ReloadCommands()
    {
        await SetBusyAsync();
        ExeCommands = await AppManagerRepo.GetExeCommandsForConfigAsync(ApiConnect.Id);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        newCommand = ExeCommandModelDB.BuildEmpty(ApiConnect.Id);
        ReloadCommands();
    }

    async Task AddNewCommand()
    {
        //await ParentPage.HoldPageUpdate(true);
        await SetBusyAsync();
        ResponseBaseModel res = await AppManagerRepo.UpdateOrCreateExeCommandAsync(newCommand);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        
        if(res.Success())
            newCommand = ExeCommandModelDB.BuildEmpty(ApiConnect.Id);

        ReloadCommands();
    }
}