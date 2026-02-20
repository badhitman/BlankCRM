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
    IClientRestToolsService RestClientRepo { get; set; } = default!;

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
        await SetBusyAsync();
        ResponseBaseModel res = await AppManagerRepo.DeleteConfigAsync(ApiConnect.Id);
        deleteInit = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
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
        await SetBusyAsync();
        AllTokens = await AppManagerRepo.GetAllConfigurationsAsync();

        if (selectedConfId == 0)
            ApiConnect.Empty();
        else
        {
            ApiRestConfigModelDB selectedConnect = AllTokens.First(x => x.Id == selectedConfId);
            ApiConnect.Update(selectedConnect);
        }

        configRef?.ResetForm();
        await SetBusyAsync(false);
    }

    async Task InitSelector()
    {
        await SetBusyAsync();
        AllTokens = await AppManagerRepo.GetAllConfigurationsAsync();
        await SetActiveHandler(AllTokens.OrderBy(x => x.Name).FirstOrDefault()?.Id ?? 0);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await InitSelector();
    }
}