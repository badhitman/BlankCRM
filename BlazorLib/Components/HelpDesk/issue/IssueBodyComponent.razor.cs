////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using BlazorLib;
using BlazorLib.Components.Rubrics;

namespace BlazorLib.Components.HelpDesk.issue;

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
    RubricNestedModel? SelectedRubric;

    void RubricSelectAction(RubricNestedModel? selectedRubric)
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
        if (CurrentUserSession is null)
            throw new Exception("CurrentUserSession is null");

        if (string.IsNullOrWhiteSpace(NameIssueEdit))
            throw new ArgumentNullException(nameof(NameIssueEdit));

        await SetBusyAsync();

        TResponseModel<int> res = await HelpDeskRepo.IssueCreateOrUpdateAsync(new()
        {
            SenderActionUserId = CurrentUserSession.UserId,
            Payload = new()
            {
                Name = NameIssueEdit,
                Description = DescriptionIssueEdit,
                ParentId = RubricIssueEdit,
                Id = Issue.Id,
            }
        });
        SnackBarRepo.ShowMessagesResponse(res.Messages);

        if (!res.Success())
        {
            await SetBusyAsync(false);
            return;
        }
        Issue.Name = NameIssueEdit;
        Issue.Description = DescriptionIssueEdit;
        Issue.RubricIssueId = RubricIssueEdit;

        IsEditMode = false;
        await SetBusyAsync(false);
    }

    void EditToggle()
    {
        IsEditMode = !IsEditMode;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        CancelEdit();

        await SetBusyAsync();
        TResponseModel<bool?> res = await SerializeStorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ParameterShowDisabledRubrics);
        TResponseModel<ModesSelectRubricsEnum?> res_ModeSelectingRubrics = await SerializeStorageRepo.ReadParameterAsync<ModesSelectRubricsEnum?>(GlobalStaticCloudStorageMetadata.ModeSelectingRubrics);

        if (!res.Success())
            SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (!res_ModeSelectingRubrics.Success())
            SnackBarRepo.ShowMessagesResponse(res_ModeSelectingRubrics.Messages);

        if (res_ModeSelectingRubrics.Response is null || (int)res_ModeSelectingRubrics.Response == 0)
            res_ModeSelectingRubrics.Response = ModesSelectRubricsEnum.AllowWithoutRubric;

        ShowDisabledRubrics = res.Response == true;
        ModeSelectingRubrics = res_ModeSelectingRubrics.Response.Value;
        SelectedRubric = Issue.RubricIssue;

        await SetBusyAsync(false);
    }
}