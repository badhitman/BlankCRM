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


    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string Label { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required StorageContextMetadataModel KeyStorage { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? HelperText { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public ModesSelectRubricsEnum ModeSelectingRubrics { get; set; } = ModesSelectRubricsEnum.AllowWithoutRubric;

    /// <inheritdoc/>
    [Parameter]
    public Action<int?>? SetValueEvent { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public bool ShowDisabledRubrics { get; set; } = true;

    /// <inheritdoc/>
    [Parameter]
    public bool IsDisabledInput { get; set; }


    int? _rubricSelected;
    void RubricSelectAction(RubricNestedModel? selectedRubric)
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

        if (SetValueEvent is not null)
            SetValueEvent(_rubricSelected);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<int?> res_RubricIssueForCreateOrder = await StoreRepo.ReadParameterAsync<int?>(KeyStorage);
        _rubricSelected = res_RubricIssueForCreateOrder.Response;
        await SetBusyAsync(false);

        if (SetValueEvent is not null)
            SetValueEvent(_rubricSelected);
    }
}