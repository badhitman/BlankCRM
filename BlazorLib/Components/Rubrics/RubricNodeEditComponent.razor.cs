////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// RubricNode: Edit
/// </summary>
public partial class RubricNodeEditComponent : BlazorBusyComponentBaseModel
{
    [Inject]
    IRubricsTransmission RubricsRepo { get; set; } = default!;


    /// <summary>
    /// ReadOnly
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// ContextName
    /// </summary>
    [Parameter, EditorRequired]
    public required string? ContextName { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required Action<int> ReloadNodeHandle { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required Action<UniversalBaseModel> ItemUpdateHandle { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required TreeItemDataRubricModel Item { get; set; }


    bool IsRenameMode;

    UniversalBaseModel? ItemModel;

    string? itemSystemName;

    /// <inheritdoc/>
    protected string DomID => $"{Item.Value?.Id}";

    bool IsEditedName => itemSystemName != ItemModel?.Name;

    //Item.MoveRowState
    static MoveRowStatesEnum[] CantUpMove => [MoveRowStatesEnum.Start, MoveRowStatesEnum.Singleton];

    static MoveRowStatesEnum[] CantDownMove => [MoveRowStatesEnum.End, MoveRowStatesEnum.Singleton];

    async Task MoveRow(DirectionsEnum dir, TreeItemDataRubricModel rubric)
    {
        if (ItemModel is null)
            return;

        await SetBusyAsync();
        ResponseBaseModel res = await RubricsRepo.RubricMoveAsync(new() { Payload = new() { Direction = dir, ObjectId = rubric.Value!.Id, ContextName = ContextName } });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        ReloadNodeHandle(ItemModel.ParentId ?? 0);
    }

    async Task SaveRubric()
    {
        if (ItemModel is null)
            throw new ArgumentNullException(nameof(ItemModel));

        if (string.IsNullOrWhiteSpace(itemSystemName))
            throw new ArgumentNullException(nameof(itemSystemName));

        IsRenameMode = false;
        ItemModel.Name = itemSystemName;

        await SetBusyAsync();
        TResponseModel<int> res = await RubricsRepo.RubricCreateOrUpdateAsync(new RubricModelDB()
        {
            Name = ItemModel.Name,
            Description = ItemModel.Description,
            Id = ItemModel.Id,
            ParentId = ItemModel.ParentId,
            ProjectId = ItemModel.ProjectId,
            SortIndex = ItemModel.SortIndex,
            IsDisabled = ItemModel.IsDisabled,
        });
        IsBusyProgress = false;
        SnackbarRepo.ShowMessagesResponse(res.Messages);
        ItemUpdateHandle(ItemModel);
    }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        ItemModel = Item.Value;
        itemSystemName = ItemModel?.Name;
    }

    /// <inheritdoc/>
    protected override void OnAfterRender(bool firstRender)
    {
        bool need_refresh = ItemModel != Item.Value;
        ItemModel = Item.Value;
        if (need_refresh)
            StateHasChanged();
    }
}