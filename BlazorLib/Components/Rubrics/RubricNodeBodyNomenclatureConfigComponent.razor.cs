////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// RubricNodeBodyNomenclatureConfigComponent
/// </summary>
public partial class RubricNodeBodyNomenclatureConfigComponent : RubricNodeBodyComponent
{
    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();
        
        await SetBusyAsync(false);
    }
}