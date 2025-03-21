////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

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


    List<string> _items = [];


    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if (KladrRow.Parents is not null)
            foreach (RootKLADRModelDB item in KladrRow.Parents)
                _items.Add($"{item.NAME} {item.SOCR}");

        _items.Add($"{KladrRow.Name} {KladrRow.Socr}");
    }
}