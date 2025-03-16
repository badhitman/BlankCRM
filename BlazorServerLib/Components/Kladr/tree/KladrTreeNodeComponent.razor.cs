////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorWebLib.Components.Kladr.tree;

/// <summary>
/// KladrNode
/// </summary>
public partial class KladrTreeNodeComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IJSRuntime JSRepo { get; set; } = default!;


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
    public required Action<RootKLADRModelDB> ItemUpdateHandle { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required TreeItemDataKladrModel Item { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required KladrMainTreeViewSetModel KladrMainTreeView { get; set; }

    /// <inheritdoc/>
    protected string DomID => $"{GetType().Name}_{Item.Value!.Id}";

    CodeKladrModel MetaType = default!;
    string? StatusElement;

    async Task GoToMap()
    {
        if (Item.Value is null)
        {
            SnackbarRepo.Error("Item.Value is null");
            return;
        }

        TreeItemDataKladrModel? parent = Item.Parent;
        string resUri = $"{Item.Value.SOCR} {Item.Value.NAME}";

        while (parent?.Value is not null)
        {
            resUri = $"{parent.Value.SOCR} {parent.Value.NAME}, {resUri}";
            parent = parent.Parent;
        }
        await JSRepo.InvokeVoidAsync("open", $"https://yandex.ru/maps?text={resUri}&source=serp_navig", "_blank");
    }

    private string GetGetCss()
    {
        if (Item.Value is ObjectMetaKLADRModel om)
        {
            return MetaType.Chain switch
            {
                KladrChainTypesEnum.RootRegions => "secondary",
                KladrChainTypesEnum.CitiesInRegion => "primary",
                KladrChainTypesEnum.PopPointsInRegion => "primary-emphasis",
                KladrChainTypesEnum.AreasInRegion => "danger",
                KladrChainTypesEnum.CitiesInArea => "info-emphasis",
                KladrChainTypesEnum.PopPointsInArea => "warning-emphasis",
                KladrChainTypesEnum.PopPointsInCity => "danger-emphasis",
                _ => "default"
            };
        }
        else if (Item.Value is StreetMetaKLADRModel sm)
        {
            return MetaType.Chain switch
            {
                KladrChainTypesEnum.StreetsInRegion => "warning",
                KladrChainTypesEnum.StreetsInCity => "success",
                KladrChainTypesEnum.StreetsInPopPoint => "info",
                _ => "default"
            };
        }

        return "success-emphasis";
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Item.Notify += EventNotify;
        MetaType = CodeKladrModel.Build(Item.Value!.CODE);
        if (Item.Value is ObjectMetaKLADRModel omm)
            StatusElement = omm.STATUS;
    }

    private void EventNotify(KladrMainTreeViewSetModel message)
    {
        StateHasChangedCall();
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        Item.Notify -= EventNotify;
        base.Dispose();
    }
}