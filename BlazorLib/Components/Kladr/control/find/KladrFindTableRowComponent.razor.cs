////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;
using static MudBlazor.CategoryTypes;

namespace BlazorLib.Components.Kladr.control.find;

/// <summary>
/// KladrFindTableRowComponent
/// </summary>
public partial class KladrFindTableRowComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required KladrResponseModel KladrRow { get; set; }

    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required string HighlightedText { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public int SkipBreadcrumbs { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public Action<KladrResponseModel>? SelectionHandler { get; set; }


    private readonly List<KladrBaseElementModel> _items = [];

    CodeKladrModel? _md;
    CodeKladrModel CurrentMd
    {
        get
        {
            _md ??= CodeKladrModel.Build(KladrRow.Code);
            return _md;
        }
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if (KladrRow.Parents is not null)
            foreach (RootKLADRModelDB item in KladrRow.Parents)
                _items.Add(KladrBaseElementModel.Build(item));//$"{item.NAME} {item.SOCR}"

        _items.Add(KladrBaseElementModel.Build(KladrRow));//$"{KladrRow.Name} {KladrRow.Socr}"
    }
}