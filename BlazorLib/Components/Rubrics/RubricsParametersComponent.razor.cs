////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// Rubrics Parameters
/// </summary>
public partial class RubricsParametersComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IParametersStorageTransmission SerializeStorageRepo { get; set; } = default!;


    bool _showDisabledRubrics;
    bool ShowDisabledRubrics
    {
        get => _showDisabledRubrics;
        set
        {
            _showDisabledRubrics = value;
            InvokeAsync(ToggleShowingDisabledRubrics);
        }
    }

    ModesSelectRubricsEnum _selectedOption;
    ModesSelectRubricsEnum SelectedOption
    {
        get => _selectedOption;
        set
        {
            _selectedOption = value;
            InvokeAsync(SaveModeSelectingRubrics);
        }
    }

    async Task SaveModeSelectingRubrics()
    {
        await SetBusyAsync();
        TResponseModel<int> res = await SerializeStorageRepo.SaveParameterAsync<ModesSelectRubricsEnum?>(SelectedOption, GlobalStaticCloudStorageMetadata.ModeSelectingRubrics, true);

        SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    async Task ToggleShowingDisabledRubrics()
    {
        await SetBusyAsync();
        TResponseModel<int> res = await SerializeStorageRepo.SaveParameterAsync<bool?>(ShowDisabledRubrics, GlobalStaticCloudStorageMetadata.ParameterShowDisabledRubrics, true);

        if (!res.Success())
            SnackBarRepo.ShowMessagesResponse(res.Messages);
        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        TResponseModel<bool?> res_ShowDisabledRubrics = await SerializeStorageRepo.ReadParameterAsync<bool?>(GlobalStaticCloudStorageMetadata.ParameterShowDisabledRubrics);
        TResponseModel<ModesSelectRubricsEnum?> res_ModeSelectingRubrics = await SerializeStorageRepo.ReadParameterAsync<ModesSelectRubricsEnum?>(GlobalStaticCloudStorageMetadata.ModeSelectingRubrics);
        await SetBusyAsync(false);
        if (!res_ShowDisabledRubrics.Success())
            SnackBarRepo.ShowMessagesResponse(res_ShowDisabledRubrics.Messages);
        if (!res_ModeSelectingRubrics.Success())
            SnackBarRepo.ShowMessagesResponse(res_ModeSelectingRubrics.Messages);

        _showDisabledRubrics = res_ShowDisabledRubrics.Response == true;

        if (res_ModeSelectingRubrics.Response is null || ((int)res_ModeSelectingRubrics.Response) == 0)
            res_ModeSelectingRubrics.Response = ModesSelectRubricsEnum.AllowWithoutRubric;

        _selectedOption = res_ModeSelectingRubrics.Response!.Value;
    }
}