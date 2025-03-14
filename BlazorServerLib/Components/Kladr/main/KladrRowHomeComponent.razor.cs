////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib;

namespace BlazorWebLib.Components.Kladr.main;

/// <inheritdoc/>
public partial class KladrRowHomeComponent
{
    /// <inheritdoc/>
    [Parameter, EditorRequired]
    public required HouseKLADRModelDB ObjectKLADR { get; set; }
}