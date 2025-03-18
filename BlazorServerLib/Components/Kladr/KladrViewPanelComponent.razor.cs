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
    IReadOnlyCollection<string>? SelectedFieldsView { get; set; }

    QueryNavKladrComponent? kladrRef;
}