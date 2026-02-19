////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using SharedLib;

namespace BlazorLib.Components.Rubrics;

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