////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

namespace BlazorRubricsLib.Components;

/// <summary>
/// RubricNodeBodyNomenclatureComponent
/// </summary>
public partial class RubricNodeBodyNomenclatureComponent : RubricNodeBodyComponent
{
    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        
        await SetBusyAsync(false);
    }
}