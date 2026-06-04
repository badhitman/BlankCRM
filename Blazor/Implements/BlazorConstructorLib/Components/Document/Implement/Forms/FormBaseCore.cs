////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorConstructorLib.Components.Document.Implement.Forms;

/// <summary>
/// FormBaseCore
/// </summary>
public abstract partial class FormBaseCore : DocumentBodyBaseComponent
{
    /// <summary>
    /// Form Metadata
    /// </summary>
    [Parameter, EditorRequired]
    public required FormFitModel FormMetadata { get; set; }

    /// <summary>
    /// TabMetadata
    /// </summary>
    [Parameter, EditorRequired]
    public required TabFitModel TabMetadata { get; set; }
}
