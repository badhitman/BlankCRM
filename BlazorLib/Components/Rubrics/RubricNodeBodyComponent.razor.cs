////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using MudBlazor;
using SharedLib;

namespace BlazorLib.Components.Rubrics;

/// <summary>
/// RubricNodeBodyComponent
/// </summary>
public partial class RubricNodeBodyComponent : BlazorBusyComponentBaseModel
{
    /// <summary>
    /// Node
    /// </summary>
    [Parameter, EditorRequired]
    public required TreeItemData<UniversalBaseModel?> Node { get; set; }
}