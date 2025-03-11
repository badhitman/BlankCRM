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
    public required Action<RootKLADRModelDB> ItemUpdateHandle { get; set; }

    /// <inheritdoc/>
    [CascadingParameter, EditorRequired]
    public required TreeItemDataKladrModel Item { get; set; }

    string GetCss => Item.Value is StreetMetaKLADRModel ? "info" : Item.Value is HouseKLADRModelDTO ? "success" : "primary";

    /// <inheritdoc/>
    protected string DomID => $"{GetType().Name}_{Item.Value!.Id}";

    KladrTypesResultsEnum? MetaType;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if(Item.Value is ObjectMetaKLADRModel omm)
        {
            MetaType = omm.MetaType;
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
}