////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.ParametersShared;

/// <summary>
/// RubricParameterStorageComponent
/// </summary>
public partial class RubricParameterStorageComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission StoreRepo { get; set; } = default!;


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


    int? _rubricSelected;
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
        SnackBarRepo.ShowMessagesResponse(res.Messages);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<int?> res_RubricIssueForCreateOrder = await StoreRepo.ReadParameterAsync<int?>(KeyStorage);
        _rubricSelected = res_RubricIssueForCreateOrder.Response;
        await SetBusyAsync(false);
    }
}