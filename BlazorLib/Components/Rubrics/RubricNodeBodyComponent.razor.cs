////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// RubricNodeBodyComponent
/// </summary>
public partial class RubricNodeBodyComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission TagsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required TreeItemData<UniversalBaseModel?> Node { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string? ContextName { get; set; }


    string? NewTagName { get; set; }
    TPaginationResponseStandardModel<TagViewModel> TagsCollection = new();


    async Task CreateNewTag()
    {
        if (string.IsNullOrWhiteSpace(NewTagName))
        {
            SnackBarRepo.Error($"string.IsNullOrWhiteSpace(NewTagName) > `{GetType().FullName}`");
            return;
        }
        if (Node.Value is null)
        {
            SnackBarRepo.Error($"Node.Value > `{GetType().FullName}`");
            return;
        }

        TagSetModel req = new()
        {
            ApplicationName = ContextName,
            Set = true,
            Name = NewTagName,
            PropertyName = GlobalStaticConstantsRoutes.Routes.MARKERS_CONTROLLER_NAME,
            PrefixPropertyName = GlobalStaticConstantsRoutes.Routes.ABOUT_CONTROLLER_NAME,
            Id = Node.Value.Id,
        };
        await SetBusyAsync();
        ResponseBaseModel res = await TagsRepo.TagSetAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        NewTagName = null;
        await ReloadTags();
        await SetBusyAsync(false);
    }

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
        if (string.IsNullOrWhiteSpace(ContextName))
        {
            SnackBarRepo.Error($"string.IsNullOrWhiteSpace(ContextName) > `{GetType().FullName}`");
            return;
        }
        await SetBusyAsync();
        TPaginationRequestStandardModel<SelectMetadataRequestModel> req = new()
        {
            Payload = new()
            {
                ApplicationsNames = [ContextName],
                OwnerPrimaryKey = Node.Value.Id,
                PropertyName = GlobalStaticConstantsRoutes.Routes.MARKERS_CONTROLLER_NAME,
                PrefixPropertyName = GlobalStaticConstantsRoutes.Routes.ABOUT_CONTROLLER_NAME,
            }
        };
        TagsCollection = await TagsRepo.TagsSelectAsync(req);
        await SetBusyAsync(false);
    }

    async Task DeleteTagHandle(MudChip<string> tagDelete)
    {
        if (string.IsNullOrWhiteSpace(tagDelete.Value))
        {
            SnackBarRepo.Error($"string.IsNullOrWhiteSpace(tagDelete.Value) > `{GetType().FullName}`");
            return;
        }
        if (Node.Value is null)
        {
            SnackBarRepo.Error($"Node.Value is null > `{GetType().FullName}`");
            return;
        }
        if (string.IsNullOrWhiteSpace(ContextName))
        {
            SnackBarRepo.Error($"string.IsNullOrWhiteSpace(ContextName) > `{GetType().FullName}`");
            return;
        }

        TagSetModel req = new()
        {
            ApplicationName = ContextName,
            Set = false,
            Name = tagDelete.Value,
            PropertyName = GlobalStaticConstantsRoutes.Routes.MARKERS_CONTROLLER_NAME,
            PrefixPropertyName = GlobalStaticConstantsRoutes.Routes.ABOUT_CONTROLLER_NAME,
            Id = Node.Value.Id,
        };
        await SetBusyAsync();
        ResponseBaseModel res = await TagsRepo.TagSetAsync(req);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await ReloadTags();
        await SetBusyAsync(false);
    }
}