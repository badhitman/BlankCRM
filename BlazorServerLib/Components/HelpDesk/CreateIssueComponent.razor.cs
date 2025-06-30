﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.HelpDesk;

/// <summary>
/// Create Issue
/// </summary>
public partial class CreateIssueComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IHelpDeskTransmission HelpDeskRepo { get; set; } = default!;

    [Inject]
    IParametersStorageTransmission SerializeStorageRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action Update { get; set; }

    /// <summary>
    /// UserIdentityId
    /// </summary>
    [Parameter, EditorRequired]
    public required string UserIdentityId { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required Action<HelpDeskJournalModesEnum> ReloadIssueJournal { get; set; }


    bool CanCreate =>
        !string.IsNullOrWhiteSpace(Name) &&
        !string.IsNullOrWhiteSpace(Description) &&
        GlobalTools.DescriptionHtmlToLinesRemark(Description).Any(x => !string.IsNullOrWhiteSpace(x)) &&
        (ModeSelectingRubrics == ModesSelectRubricsEnum.AllowWithoutRubric || (SelectedRubric is not null && SelectedRubric.Id > 0))
        ;

    string? Name;
    string? Description;
    bool _showCreateIssue;
    bool IsEditMode { get; set; } = false;

    ModesSelectRubricsEnum ModeSelectingRubrics;
    bool ShowDisabledRubrics;
    UniversalBaseModel? SelectedRubric;

    async Task CreateIssue()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            SnackBarRepo.Error("Не указано имя");
            return;
        }

        TResponseModel<int> res = await HelpDeskRepo.IssueCreateOrUpdateAsync(new()
        {
            SenderActionUserId = UserIdentityId,
            Payload = new()
            {
                ParentId = SelectedRubric?.Id,
                Name = Name,
                Description = Description,
            }
        });
        IsBusyProgress = false;
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Success())
            ToggleMode();
        Update();
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<bool?> res_ShowCreatingIssue = await SerializeStorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ShowCreatingIssue);
        _showCreateIssue = res_ShowCreatingIssue.Success() && res_ShowCreatingIssue.Response == true;
        TResponseModel<bool?> res = await SerializeStorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ParameterShowDisabledRubrics);
        TResponseModel<ModesSelectRubricsEnum?> res_ModeSelectingRubrics = await SerializeStorageRepo.ReadParameterAsync<ModesSelectRubricsEnum?>(GlobalStaticCloudStorageMetadata.ModeSelectingRubrics);
        IsBusyProgress = false;

        if (!res.Success())
            SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (!res_ModeSelectingRubrics.Success())
            SnackBarRepo.ShowMessagesResponse(res_ModeSelectingRubrics.Messages);

        if (res_ModeSelectingRubrics.Response is null || (int)res_ModeSelectingRubrics.Response == 0)
            res_ModeSelectingRubrics.Response = ModesSelectRubricsEnum.AllowWithoutRubric;

        ShowDisabledRubrics = res.Response == true;
        ModeSelectingRubrics = res_ModeSelectingRubrics.Response.Value;
    }

    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        SelectedRubric = selectedRubric;
        StateHasChanged();
    }

    void ToggleMode()
    {
        IsEditMode = !IsEditMode;
        if (!IsEditMode)
        {
            Description = null;
            Name = null;
            SelectedRubric = null;
            return;
        }
    }
}