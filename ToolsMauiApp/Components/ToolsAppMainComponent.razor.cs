////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib.Components.ToolsApp;
using BlazorLib;
using SharedLib;

namespace ToolsMauiApp.Components;

/// <summary>
/// ToolsAppMainComponent
/// </summary>
public partial class ToolsAppMainComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IClientHTTPRestService RestClientRepo { get; set; } = default!;

    [Inject]
    IToolsAppManager AppManagerRepo { get; set; } = default!;

    [Inject]
    ApiRestConfigModelDB ApiConnect { get; set; } = default!;


    ApiRestConfigModelDB[] AllTokens = [];
    ConnectionConfigComponent? configRef;

    bool deleteInit;

#if DEBUG
    bool IsDebug = true;
#else
    bool IsDebug = false;
#endif

    async Task DeleteConnectionConfig()
    {
        if (!deleteInit)
        {
            deleteInit = true;
            return;
        }
        await SetBusy();
        ResponseBaseModel res = await AppManagerRepo.DeleteConfig(ApiConnect.Id);
        deleteInit = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        await SetBusy(false);
        await InitSelector();
    }

    /// <summary>
    /// SelectedOfferId
    /// </summary>
    public int SelectedConfId
    {
        get => ApiConnect.Id;
        set => InvokeAsync(() => SetActiveHandler(value));
    }

    /// <summary>
    /// SetActiveHandler
    /// </summary>
    public async Task SetActiveHandler(int selectedConfId)
    {
        await SetBusy();
        AllTokens = await AppManagerRepo.GetAllConfigurations();

        if (selectedConfId == 0)
            ApiConnect.Empty();
        else
        {
            ApiRestConfigModelDB selectedConnect = AllTokens.First(x => x.Id == selectedConfId);
            ApiConnect.Update(selectedConnect);
        }

        configRef?.ResetForm();
        await SetBusy(false);
    }

    async Task InitSelector()
    {
        await SetBusy();
        AllTokens = await AppManagerRepo.GetAllConfigurations();
        await SetActiveHandler(AllTokens.OrderBy(x => x.Name).FirstOrDefault()?.Id ?? 0);
        await SetBusy(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await InitSelector();
    }
}