////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using BlazorLib;
using SharedLib;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorWebLib.Components.Kladr;

/// <summary>
/// KladrNode
/// </summary>
public partial class KladrNodeComponent : BlazorBusyComponentBaseAuthModel
{
    [Inject]
    IKladrNavigationService KladrNavRepo { get; set; } = default!;

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
    protected string DomID => $"{GetType().Name}_{Item.Value!.Id}";

    KladrTypesResultsEnum? MetaType;
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
            return om.MetaType switch
            {
                KladrTypesResultsEnum.RootRegions => "secondary",
                KladrTypesResultsEnum.CitiesInRegion => "primary",
                KladrTypesResultsEnum.PopPointsInRegion => "primary-emphasis",
                KladrTypesResultsEnum.AreasInRegion => "danger",
                KladrTypesResultsEnum.CitiesInArea => "info-emphasis",
                KladrTypesResultsEnum.PopPointsInArea => "warning-emphasis",
                KladrTypesResultsEnum.PopPointsInCity => "danger-emphasis",
                _ => "default"
            };
        }
        else if (Item.Value is StreetMetaKLADRModel sm)
        {
            return sm.MetaType switch
            {
                KladrTypesResultsEnum.StreetsInRegion => "warning",
                KladrTypesResultsEnum.StreetsInCity => "success",
                KladrTypesResultsEnum.StreetsInPopPoint => "info",
                _ => "default"
            };
        }

        return "success-emphasis";
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Item.Value is ObjectMetaKLADRModel omm)
        {
            MetaType = omm.MetaType;
            StatusElement = omm.STATUS;
        }
        else if (Item.Value is StreetMetaKLADRModel smm)
        {
            MetaType = smm.MetaType;
        }
        else// if(Item.Value is HouseKLADRModelDTO)
        {
            MetaType = KladrTypesResultsEnum.HousesInStreet;
        }
    }


#if DEBUG
    bool IsDebug = true;
#else
    bool IsDebug = false;
#endif
}