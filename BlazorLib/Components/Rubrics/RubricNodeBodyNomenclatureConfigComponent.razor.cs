////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// RubricNodeBodyNomenclatureConfigComponent
/// </summary>
public partial class RubricNodeBodyNomenclatureConfigComponent : RubricNodeBodyComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required int ProjectId { get; set; }

    /// <inheritdoc/>
    protected override async Task OnInitializedAsync()
    {
        await SetBusyAsync();
        await base.OnInitializedAsync();

        await SetBusyAsync(false);
    }
}