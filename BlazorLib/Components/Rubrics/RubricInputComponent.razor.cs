////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Rubrics
{
    public partial class RubricInputComponent
    {
        [Inject]
        IRubricsTransmission RubricsRepo { get; set; } = default!;


        /// <inheritdoc/>
        [Parameter]
        public string Title { get; set; } = "Рубрика/категория";

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


        readonly List<(int parentId, List<UniversalBaseModel> nestedElements)> SelectSource = [];
        List<RubricStandardModel> RubricHierarchy { get; set; } = [];
        (RubricStandardModel rubric, int indexHierarchy)? SelectedElement;
        bool IsEdited 
        { 
            get 
            {
                if(SelectedElement is null && RubricInitial == 0)
                    return false;
                else if(SelectedElement is null || RubricInitial == 0)
                    return true;

                return SelectedElement.Value.rubric.Id == RubricInitial;
            } 
        }


        async void HandleSelectionChange(ChangeEventArgs e, (int parentId, List<UniversalBaseModel> nestedElements) _kvp)
        {
            int
                _valId = int.Parse(e.Value!.ToString()!),
                srcIndex = SelectSource.FindIndex(x => x.parentId == _kvp.parentId);

            if (srcIndex + 1 < SelectSource.Count)
                SelectSource.RemoveRange(srcIndex + 1, SelectSource.Count - (srcIndex + 1));

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
            SelectedElement = (_rubricGet, srcIndex);
            await SetBusyAsync(false);
            SelectRubricsHandle(_rubricGet);
        }

        bool IsSelectedElement((int parentId, List<UniversalBaseModel> nestedElements) _kvp, UniversalBaseModel rubricNode)
        {
            return RubricHierarchy.FirstOrDefault(x => x.ParentId == _kvp.parentId)?.Id == rubricNode.Id;
        }

        async Task HierarchyUpdateAsync(int _id)
        {
            TResponseModel<List<RubricStandardModel>> dump_rubric = await RubricsRepo.RubricReadWithParentsHierarchyAsync(_id);
            SnackBarRepo.ShowMessagesResponse(dump_rubric.Messages);
            if (dump_rubric.Response is null)
            {
                SnackBarRepo.Error("dump_rubric.Response is null");
                throw new Exception();
            }
            RubricHierarchy = dump_rubric.Response;
        }

        /// <inheritdoc/>
        public async Task SetRubric(int rubricId)
        {
            SelectSource.Clear();
            SelectSource.Add((0, await RubricsRepo.RubricsChildListAsync(new RubricsListRequestModel() { ContextName = ContextName, Request = 0 })));
            SelectedElement = null;
            if (rubricId < 1)
            {
                StateHasChanged();
                return;
            }

            await SetBusyAsync();
            await HierarchyUpdateAsync(rubricId);
            IEnumerable<RubricStandardModel> _q = RubricHierarchy
                .Where(x => x.ParentId.HasValue && x.ParentId.Value > 0);

            IEnumerable<int> _ids = _q
                .Select(x => x.ParentId!.Value);

            if (_ids.Any())
            {
                TResponseModel<List<RubricStandardModel>> res = await RubricsRepo.RubricsGetAsync(_ids);
                SnackBarRepo.ShowMessagesResponse(res.Messages);
                if (res.Response is null)
                {
                    SnackBarRepo.Error("result [Response]: is null");
                    throw new Exception();
                }
                foreach (RubricStandardModel _r in _q)
                    SelectSource.Add((_r.ParentId!.Value, [.. res.Response.Where(x => x.ParentId == _r.ParentId)]));
            }

            await SetBusyAsync(false);
            SelectedElement = (RubricHierarchy.Last(), SelectSource.Count - 1);
        }

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await SetBusyAsync();
            SelectSource.Add((0, await RubricsRepo.RubricsChildListAsync(new RubricsListRequestModel() { ContextName = ContextName, Request = 0 })));

            if (RubricInitial != 0)
            {
                await HierarchyUpdateAsync(RubricInitial);
                IEnumerable<RubricStandardModel> _q = RubricHierarchy
                    .Where(x => x.ParentId.HasValue && x.ParentId.Value > 0);

                IEnumerable<int> _ids = _q
                    .Select(x => x.ParentId!.Value);

                if (_ids.Any())
                {
                    TResponseModel<List<RubricStandardModel>> res = await RubricsRepo.RubricsGetAsync(_ids);
                    SnackBarRepo.ShowMessagesResponse(res.Messages);
                    if (res.Response is null)
                    {
                        SnackBarRepo.Error("result [Response]: is null");
                        throw new Exception();
                    }
                    foreach (RubricStandardModel _r in _q)
                        SelectSource.Add((_r.ParentId!.Value, [.. res.Response.Where(x => x.ParentId == _r.ParentId)]));
                }
            }

            await SetBusyAsync(false);
        }
    }
}