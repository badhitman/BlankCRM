////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;

namespace BlazorWebLib.Components.Kladr;

/// <summary>
/// KladrNode: Edit
/// </summary>
public partial class KladrNodeEditComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IKladrNavigationService KladrNavRepo { get; set; } = default!;


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


    UniversalBaseModel? ItemModel;

    string? itemSystemName;

    /// <inheritdoc/>
    protected string DomID => $"{Item.Value?.Id}";

    bool IsEditedName => itemSystemName != ItemModel?.Name;


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