////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

public partial class RubricNodeBodyComponent
{
    /// <summary>
    /// Node
    /// </summary>
    [Parameter, EditorRequired]
    public required TreeItemData<UniversalBaseModel?> Node { get; set; }
}