////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using BlazorLib.Components.Rubrics;

namespace BlazorLib.Components.StockSharp;

/// <summary>
/// InstrumentsForRubricComponent
/// </summary>
public partial class InstrumentsForRubricComponent : RubricNodeBodyComponent
{
    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await SetBusyAsync();

        await SetBusyAsync(false);
    }
}