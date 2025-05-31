////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.HelpDesk;
using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.ParametersShared;

/// <summary>
/// RubricParameterStorageComponent
/// </summary>
public partial class RubricParameterStorageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IStorageTransmission StoreRepo { get; set; } = default!;

    [Inject]
    IRubricsTransmission HelpDeskRepo { get; set; } = default!;


    /// <summary>
    /// Label
    /// </summary>
    [Parameter, EditorRequired]
    public required string Label { get; set; }

    /// <summary>
    /// KeyStorage
    /// </summary>
    [Parameter, EditorRequired]
    public required StorageMetadataModel KeyStorage { get; set; }

    /// <summary>
    /// HelperText
    /// </summary>
    [Parameter]
    public string? HelperText { get; set; }

    /// <summary>
    /// ModeSelectingRubrics
    /// </summary>
    [Parameter]
    public ModesSelectRubricsEnum ModeSelectingRubrics { get; set; } = ModesSelectRubricsEnum.AllowWithoutRubric;

    /// <summary>
    /// ShowDisabledRubrics
    /// </summary>
    [Parameter]
    public bool ShowDisabledRubrics { get; set; } = true;


    RubricSelectorComponent? ref_rubric;
    int? _rubricSelected;
    List<RubricIssueHelpDeskModelDB>? RubricMetadataShadow;
    void RubricSelectAction(UniversalBaseModel? selectedRubric)
    {
        _rubricSelected = selectedRubric?.Id;
        InvokeAsync(SaveRubric);
        StateHasChanged();
    }

    async void SaveRubric()
    {
        await SetBusyAsync();
        TResponseModel<int> res = await StoreRepo.SaveParameterAsync(_rubricSelected, KeyStorage, true);
        await SetBusyAsync(false);
        SnackbarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<int?> res_RubricIssueForCreateOrder = await StoreRepo.ReadParameterAsync<int?>(KeyStorage);
        _rubricSelected = res_RubricIssueForCreateOrder.Response;
        if (ref_rubric is not null && _rubricSelected.HasValue)
        {
            TResponseModel<List<RubricIssueHelpDeskModelDB>> res = await HelpDeskRepo.RubricReadAsync(_rubricSelected.Value);
            await SetBusyAsync(false);
            SnackbarRepo.ShowMessagesResponse(res.Messages);
            RubricMetadataShadow = res.Response;
            if (RubricMetadataShadow is not null && RubricMetadataShadow.Count != 0)
            {
                RubricIssueHelpDeskModelDB current_element = RubricMetadataShadow.Last();

                await ref_rubric.OwnerRubricSet(current_element.ParentId ?? 0);
                await ref_rubric.SetRubric(current_element.Id, RubricMetadataShadow);
                ref_rubric.StateHasChangedCall();
            }
        }
        else
            await SetBusyAsync(false);
    }
}