////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// Rubric selector
/// </summary>
public partial class RubricSelectorComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required Action<UniversalBaseModel?> SelectRubricsHandle { get; set; }

    /// <summary>
    /// StartRubric
    /// </summary>
    [Parameter]
    public int? StartRubric { get; set; }

    [CascadingParameter]
    List<RubricStandardModel>? RubricMetadataShadow { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int ParentRubric { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required bool ShowDisabledRubrics { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ModesSelectRubricsEnum ModeSelectingRubrics { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Рубрика/категория обращения";

    /// <summary>
    /// ContextName
    /// </summary>
    [Parameter]
    public string? ContextName { get; set; }


    RubricSelectorComponent? childSelector;

    List<UniversalBaseModel>? CurrentRubrics;

    int _selectedRubricId;
    /// <summary>
    /// Selected Rubric Id
    /// </summary>
    public int SelectedRubricId
    {
        get => _selectedRubricId;
        set
        {
            _selectedRubricId = value;
            if (childSelector is not null)
                InvokeAsync(async () => await childSelector.OwnerRubricSet(_selectedRubricId));
            SelectRubricsHandle(_selectedRubricId > 0 ? CurrentRubrics?.FirstOrDefault(x => x.Id == _selectedRubricId) : null);
        }
    }

    /// <summary>
    /// Сброс состояния селектора.
    /// </summary>
    public async Task OwnerRubricSet(int ownerRubricId)
    {
        if (ParentRubric != ownerRubricId)
        {
            ParentRubric = ownerRubricId;
            _selectedRubricId = 0;
        }

        await SetBusyAsync();
        CurrentRubrics = await RubricsRepo.RubricsListAsync(new() { Request = ownerRubricId, ContextName = ContextName });

        await SetBusyAsync(false);
    }

    /// <inheritdoc/>
    public async Task SetRubric(int rubric_id, List<RubricStandardModel>? set_rubricMetadataShadow)
    {
        _selectedRubricId = rubric_id;

        if (set_rubricMetadataShadow is not null)
            RubricMetadataShadow = set_rubricMetadataShadow;
        else
        {
            await SetBusyAsync();

            TResponseModel<List<RubricStandardModel>> dump_rubric = await RubricsRepo.RubricReadAsync(rubric_id);
            RubricMetadataShadow = dump_rubric.Response;
            SnackBarRepo.ShowMessagesResponse(dump_rubric.Messages);
            IsBusyProgress = false;
        }
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await OwnerRubricSet(ParentRubric);
        if (ParentRubric == 0 && StartRubric.HasValue)
            await SetRubric(StartRubric.Value, RubricMetadataShadow);
        else if (RubricMetadataShadow is not null && RubricMetadataShadow.Count != 0 && StartRubric.HasValue && StartRubric.Value > 0)
            _selectedRubricId = RubricMetadataShadow?.LastOrDefault()?.Id ?? 0;

        if (ParentRubric == 0 && ParentRubric == StartRubric && ModeSelectingRubrics == ModesSelectRubricsEnum.SelectAny && CurrentRubrics is not null && CurrentRubrics.Count != 0)
            SelectedRubricId = CurrentRubrics.First().Id;
    }
}