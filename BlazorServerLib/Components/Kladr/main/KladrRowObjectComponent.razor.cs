////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorWebLib.Components.Kladr.main;

/// <inheritdoc/>
public partial class KladrRowObjectComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required ObjectKLADRModelDB ObjectKLADR { get; set; }
}