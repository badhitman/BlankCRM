////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorWebLib.Components.Kladr.main;

namespace BlazorWebLib.Components.Kladr;

/// <summary>
/// KladrViewPanelComponent
/// </summary>
public partial class KladrViewPanelComponent
{
    IReadOnlyCollection<string>? _selectedFieldsView;
    IReadOnlyCollection<string>? SelectedFieldsView
    {
        get => _selectedFieldsView;
        set
        {
            _selectedFieldsView = value;
            kladrRef?.Reload();
        }
    }

    QueryNavKladrComponent? kladrRef;
}