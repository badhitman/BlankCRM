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
    IServerToolsService ToolsLocalRepo { get; set; } = default!;

    [Inject]
    IClientHTTPRestService ToolsExtRepo { get; set; } = default!;

    [Inject]
    IToolsAppManager AppManagerRepo { get; set; } = default!;


    /// <summary>
    /// Home page
    /// </summary>
    [Parameter, EditorRequired]
    public required ToolsAppMainComponent ParentPage { get; set; }


    ExeCommandModelDB[] ExeCommands { get; set; } = [];

    private ExeCommandModelDB newCommand = default!;

    async void ReloadCommands()
    {
        await SetBusy();
        ExeCommands = await AppManagerRepo.GetExeCommandsForConfig(ApiConnect.Id);
        await SetBusy(false);
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
        await SetBusy();
        ResponseBaseModel res = await AppManagerRepo.UpdateOrCreateExeCommand(newCommand);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        
        if(res.Success())
            newCommand = ExeCommandModelDB.BuildEmpty(ApiConnect.Id);

        ReloadCommands();
    }
}