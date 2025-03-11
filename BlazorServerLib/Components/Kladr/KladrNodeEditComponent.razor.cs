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
    /// ContextName
    /// </summary>
    [Parameter, EditorRequired]
    public required string? ContextName { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required Action<int> ReloadNodeHandle { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required Action<ObjectKLADRModelDB> ItemUpdateHandle { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required TreeItemDataKladrModel Item { get; set; }


    ObjectKLADRModelDB ItemModel = default!;

    /// <inheritdoc/>
    protected string DomID => $"{Item.Value!.Id}";


    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        ItemModel = Item.Value ?? throw new Exception("Item.Value is NULL");
    }

    /// <inheritdoc/>
    protected override void OnAfterRender(bool firstRender)
    {
        bool need_refresh = ItemModel != Item.Value;
        ItemModel = Item.Value ?? throw new Exception("Item.Value is NULL");
        if (need_refresh)
            StateHasChanged();
    }
}