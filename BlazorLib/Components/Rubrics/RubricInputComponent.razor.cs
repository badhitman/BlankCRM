////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// RubricInputComponent
/// </summary>
public partial class RubricInputComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    /// <inheritdoc/>
    [Parameter]
    public string Title { get; set; } = "Рубрика/категория";

    /// <inheritdoc/>
    [Parameter]
    public string? NullElementText { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int RubricInitial { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public string? ContextName { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required bool ShowDisabledRubrics { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required Action<UniversalBaseModel?> SelectRubricsHandle { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public ModesSelectRubricsEnum ModeSelectingRubrics { get; set; }


    readonly List<(int parentId, List<UniversalBaseModel> nestedElements)> SelectSource = [];
    List<RubricStandardModel> RubricHierarchy = [];

    /// <summary>
    /// Выбранная рубрика
    /// </summary>
    /// <remarks>
    /// null - если не выбрано
    /// </remarks>
    int? SelectedRubricId;

    bool IsEdited
    {
        get
        {
            if (SelectedRubricId is null && RubricInitial == 0)
                return false;
            else if (SelectedRubricId is null || RubricInitial == 0)
                return true;

            return SelectedRubricId != RubricInitial;
        }
    }

    #region NullElement
    string TitleNullElement((int parentId, List<UniversalBaseModel> nestedElements) kvp, UniversalBaseModel? currentSelected)
    {
        return currentSelected is null
            ? NullElementText ?? ""
            : "";
    }

    bool ShowNullElement((int parentId, List<UniversalBaseModel> nestedElements) kvp, UniversalBaseModel? currentSelected)
                => ModeSelectingRubrics == ModesSelectRubricsEnum.AllowWithoutRubric || (kvp.parentId == 0 && SelectedRubricId is null);

    bool DisablesNullElement((int parentId, List<UniversalBaseModel> nestedElements) kvp, UniversalBaseModel? currentSelected)
    {
        if (ModeSelectingRubrics == ModesSelectRubricsEnum.AllowWithoutRubric)
            return false;

        return true;
    }

    bool SelectedNullElement((int parentId, List<UniversalBaseModel> nestedElements) kvp, UniversalBaseModel? currentSelected)
    {
        return currentSelected is null;
    }
    #endregion


    async void HandleSelectionChange(ChangeEventArgs e, (int parentId, List<UniversalBaseModel> nestedElements) _kvp)
    {
        int
            _valId = int.Parse(e.Value!.ToString()!),
            srcIndex = SelectSource.FindIndex(x => x.parentId == _kvp.parentId);

        if (srcIndex + 1 < SelectSource.Count)
            SelectSource.RemoveRange(srcIndex + 1, SelectSource.Count - (srcIndex + 1));

        if (_valId == 0)
        {
            SelectedRubricId = null;
            SelectRubricsHandle(null);
            return;
        }


        await SetBusyAsync();
        TResponseModel<List<RubricStandardModel>> res = await RubricsRepo.RubricsGetAsync([_valId]);
        SnackBarRepo.ShowMessagesResponse(res.Messages);
        if (res.Response is null)
        {
            SnackBarRepo.Error("result [Response]: is null");
            throw new Exception();
        }
        RubricStandardModel _rubricGet = res.Response.First();
        if (_rubricGet.NestedRubrics is not null && _rubricGet.NestedRubrics.Count != 0)
            SelectSource.Add((_valId, [.. _rubricGet.NestedRubrics.Select(x => x)]));

        await HierarchyUpdateAsync(_valId);
        SelectedRubricId = _valId;
        await SetBusyAsync(false);
        SelectRubricsHandle(_rubricGet);
    }

    /// <inheritdoc/>
    public async Task SetRubric(int rubricId)
    {
        SelectSource.Clear();
        SelectSource.Add((0, await RubricsRepo.RubricsChildListAsync(new RubricsListRequestModel() { ContextName = ContextName, Request = 0 })));
        SelectedRubricId = null;
        if (rubricId < 1)
        {
            StateHasChanged();
            return;
        }

        await SetBusyAsync();
        await Actualize(rubricId);
        SelectedRubricId = RubricHierarchy.Last().Id;
        await SetBusyAsync(false);
    }

    async Task HierarchyUpdateAsync(int _id)
    {
        SelectedRubricId = null;
        TResponseModel<List<RubricStandardModel>> dump_rubric = await RubricsRepo.RubricReadWithParentsHierarchyAsync(_id);
        SnackBarRepo.ShowMessagesResponse(dump_rubric.Messages);
        if (dump_rubric.Response is null)
        {
            SnackBarRepo.Error("dump_rubric.Response is null");
            throw new Exception();
        }
        RubricHierarchy = dump_rubric.Response;
        if (_id > 0)
            SelectedRubricId = RubricHierarchy.Last().Id;
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();
        SelectSource.Add((0, await RubricsRepo.RubricsChildListAsync(new RubricsListRequestModel() { ContextName = ContextName, Request = 0 })));

        if (RubricInitial != 0)
            await Actualize(RubricInitial);

        await SetBusyAsync(false);
    }

    async Task Actualize(int rubricId)
    {
        await HierarchyUpdateAsync(rubricId);
        IEnumerable<RubricStandardModel> _q = RubricHierarchy
            .Where(x => x.ParentId.HasValue && x.ParentId.Value > 0);

        IEnumerable<int> _idsParents = _q
            .Select(x => x.ParentId!.Value);

        if (_idsParents.Any())
        {
            TResponseModel<List<RubricStandardModel>> res = await RubricsRepo.RubricsGetAsync(_idsParents);
            SnackBarRepo.ShowMessagesResponse(res.Messages);
            if (res.Response is null)
            {
                SnackBarRepo.Error("result [Response]: is null");
                throw new Exception();
            }
            foreach (RubricStandardModel _r in _q)
                SelectSource.Add((_r.ParentId!.Value, [.. res.Response.First(x => x.Id == _r.ParentId).NestedRubrics!]));
        }

    }
}