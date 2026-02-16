////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// RubricNodeBodyNomenclatureConfigComponent
/// </summary>
public partial class RubricNodeBodyNomenclatureConfigComponent : RubricNodeBodyComponent
{

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        await ReloadTags();
        await SetBusyAsync(false);
    }

    async Task ReloadTags()
    {
        if (Node.Value is null)
        {
            SnackBarRepo.Error($"Node.Value is null > `{GetType().FullName}`");
            return;
        }

        await SetBusyAsync();
        TPaginationRequestStandardModel<SelectMetadataRequestModel> req = new()
        {
            Payload = new()
            {
                ApplicationsNames = string.IsNullOrWhiteSpace(ContextName) ? ["", null] : [ContextName],
                OwnerPrimaryKey = Node.Value.Id,
                PropertyName = GlobalStaticConstantsRoutes.Routes.MARKERS_CONTROLLER_NAME,
                PrefixPropertyName = GlobalStaticConstantsRoutes.Routes.ABOUT_CONTROLLER_NAME,
            }
        };
        
        await SetBusyAsync(false);
    }
}