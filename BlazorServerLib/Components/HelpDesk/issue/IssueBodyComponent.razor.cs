////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using BlazorLib;
using BlazorLib.Components.Rubrics;

namespace BlazorWebLib.Components.HelpDesk.issue;

/// <summary>
/// IssueBodyComponent
/// </summary>
public partial class IssueBodyComponent : IssueWrapBaseModel
{
    [Inject]
    IParametersStorageTransmission SerializeStorageRepo { get; set; } = default!;


    /// <summary>
    /// IssueSource
    /// </summary>
    [Parameter, CascadingParameter]
    public IssueHelpDeskModelDB? IssueSource { get; set; }

    bool CanSave =>
        !string.IsNullOrWhiteSpace(NameIssueEdit) &&
        !string.IsNullOrWhiteSpace(DescriptionIssueEdit) &&
        GlobalTools.DescriptionHtmlToLinesRemark(DescriptionIssueEdit).Any(x => !string.IsNullOrWhiteSpace(x)) &&
        (ModeSelectingRubrics == ModesSelectRubricsEnum.AllowWithoutRubric || (SelectedRubric is not null && SelectedRubric.Id > 0))
        ;

    string? NameIssueEdit { get; set; }
    string? DescriptionIssueEdit { get; set; }

    int? RubricIssueEdit { get; set; }

    bool IsEdited =>
        NameIssueEdit != Issue.Name ||
        DescriptionIssueEdit != Issue.Description ||
        RubricIssueEdit != Issue.RubricIssueId;

    MarkupString DescriptionHtml => (MarkupString)(Issue.Description ?? "");

    ModesSelectRubricsEnum ModeSelectingRubrics;
    bool ShowDisabledRubrics;
    bool IsEditMode { get; set; }
    UniversalBaseModel? SelectedRubric;
    RubricSelectorComponent? rubricSelector_ref;
    List<RubricStandardModel>? RubricMetadataShadow;

    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        SelectedRubric = selectedRubric;
        RubricIssueEdit = SelectedRubric?.Id;
        StateHasChanged();
    }

    void CancelEdit()
    {
        NameIssueEdit = Issue.Name;
        DescriptionIssueEdit = Issue.Description;
        RubricIssueEdit = Issue.RubricIssueId;

        IsEditMode = false;
    }

    async Task SaveIssue()
    {
        if (string.IsNullOrWhiteSpace(NameIssueEdit))
            throw new ArgumentNullException(nameof(NameIssueEdit));

        await SetBusyAsync();

        TResponseModel<int> res = await HelpDeskRepo.IssueCreateOrUpdateAsync(new()
        {
            SenderActionUserId = CurrentUserSession!.UserId,
            Payload = new()
            {
                Name = NameIssueEdit,
                Description = DescriptionIssueEdit,
                ParentId = RubricIssueEdit,
                Id = Issue.Id,
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        IsBusyProgress = false;
        if (!res.Success())
            return;

        Issue.Name = NameIssueEdit;
        Issue.Description = DescriptionIssueEdit;
        Issue.RubricIssueId = RubricIssueEdit;

        IsEditMode = false;
    }

    async Task EditToggle()
    {
        IsEditMode = !IsEditMode;

        if (rubricSelector_ref is not null && Issue.RubricIssueId is not null)
        {
            await SetBusyAsync();

            TResponseModel<List<RubricStandardModel>> res = await RubricsRepo.RubricReadAsync(Issue.RubricIssueId.Value);
            IsBusyProgress = false;
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            RubricMetadataShadow = res.Response;
            if (RubricMetadataShadow is not null && RubricMetadataShadow.Count != 0)
            {
                RubricStandardModel current_element = RubricMetadataShadow.Last();

                await rubricSelector_ref.ParentRubricSet(current_element.ParentId ?? 0);
                await rubricSelector_ref.SetRubric(current_element.Id, RubricMetadataShadow);
                rubricSelector_ref.StateHasChangedCall();
            }
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        CancelEdit();

        await SetBusyAsync();
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
        SelectedRubric = Issue.RubricIssue;
    }
}